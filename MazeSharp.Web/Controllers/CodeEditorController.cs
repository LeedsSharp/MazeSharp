using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MazeSharp.Domain.Extensions;
using MazeSharp.Game;
using MazeSharp.Web.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


using MazeSharp.Web.ViewModels.CodeEditor;

namespace MazeSharp.Web.Controllers
{
    public class CodeEditorController : Controller
    {
        private const string StartSource = "using MazeSharp.Interfaces;\r\n\r\nnamespace YourTeamName\r\n{\r\n    public class StandStillPlayer : IPlayer\r\n    {\r\n        public ICell Move(IMaze maze)\r\n        {\r\n            return maze.CurrentPosition;\r\n        }\r\n    }\r\n}";
        private readonly IPlayerSavingService<IPlayer> _playerSavingService; 

        public CodeEditorController()
        {
            _playerSavingService = new PlayerSavingService<IPlayer>();
        }

        public ActionResult Index(Guid? s = null)
        {
            var viewModel = new IndexViewModel("Code Editor")
            {
                Source = s.HasValue
                    ? (string) MemoryCache.Default[s.ToString()]
                    : StartSource
            };


            return View(viewModel);
        }

        // Todo: Move this to be an ajax action

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var referenceNames = Regex.Matches(source, "using ([^;]+)").Cast<Match>().Select(m => m.Groups[1].Value);
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.LoadWithPartialName("System").Location),
                MetadataReference.CreateFromFile(Assembly.LoadWithPartialName("System.Core").Location)
            };
            foreach (var reference in referenceNames)
            {
                try
                {
                    references.Add(MetadataReference.CreateFromFile(Assembly.LoadWithPartialName(reference).Location));
                }
                catch (NullReferenceException)
                {
                }
            }
            var compilation = CSharpCompilation.Create("IPlayer", new[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                
                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(ms.ToArray());
                    var players = ExtractPlayersFromAssembly(assembly).ToList();
                    SavePlayers(players);

                    var sourceGuid = Guid.NewGuid().ToString();
                    MemoryCache.Default.Add(sourceGuid, source, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
                    return RedirectToAction("Index", new {s = sourceGuid});
                }

                var viewModel = new IndexViewModel("Code Editor")
                {
                    Diagnostics = result.Diagnostics.Select(d => d.ToString()).ToList(),
                    Source = source
                };

                return View(viewModel);
            }
        }

        private static IEnumerable<IPlayer> ExtractPlayersFromAssembly(Assembly playerAssembly)
        {
            foreach (var type in playerAssembly.GetTypes().OrderBy(t => t.Name))
            {
                if (type.GetInterface("IPlayer") != null)
                {
                    yield return Activator.CreateInstance(type) as IPlayer;
                }
            }
        }

        private void SavePlayers(IList<IPlayer> players)
        {
            var teamName = players.First().GetTeamName();
            foreach (var player in players)
            {
                _playerSavingService.SavePlayer(teamName, player.GetName(), player);
            }
        }
    }
}