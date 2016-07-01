using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Web.Mvc;
using MazeSharp.Domain;
using MazeSharp.Domain.Extensions;
using MazeSharp.Game;
using MazeSharp.Web.Services;
using MazeSharp.Web.ViewModels.Home;
using Newtonsoft.Json;

namespace MazeSharp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayerSavingService<IPlayer> playerSavingService;

        public HomeController()
        {
            playerSavingService = new PlayerSavingService<IPlayer>();
        }


        #region Actions
        public ActionResult Index(string player, string team)
        {
            IPlayer loadedPlayer = null;
            string message = null;

            if (!string.IsNullOrWhiteSpace(player) && !string.IsNullOrWhiteSpace(team))
            {
                loadedPlayer = playerSavingService.LoadPlayer(team, player);
                if (loadedPlayer != null)
                {
                    playerSavingService.SaveCurrentPlayerWithState(loadedPlayer);
                    message = $"Loaded player {player} from team {team}";
                }
            }

            if (loadedPlayer == null)
            {
                loadedPlayer = playerSavingService.LoadCurrentPlayerWithState();
            }

            var viewModel = new IndexViewModel("Maze Sharp")
            {         
                Message = message,
                LoadedPlayer = loadedPlayer.GetName(),
                Teams = GetCurrentTeams().ToList(),
                MazeJson = LoadMazeJson()
            };

            return View(viewModel);
        }

        private IEnumerable<TeamViewModel> GetCurrentTeams()
        {
            var teams = playerSavingService.GetTeams();
            if (teams == null) yield break;

            foreach (var team in teams)
            {
                var players = playerSavingService.GetPlayerNamesForTeam(team);
                yield return new TeamViewModel
                {
                    Name = team,
                    Players = players
                };
            }
        }

        public ContentResult Generate(int width, int height, string perfect)
        {
            var maze = new Maze(width, height);
            maze.Generate(DateTime.Now.Millisecond, perfect == "on" | string.IsNullOrEmpty(perfect));

            SaveMaze(maze);

            var json = JsonConvert.SerializeObject(maze);
            SaveMazeJson(json);
            return Content(json, "application/json");
        }

        public ContentResult Move(int currentX, int currentY)
        {
            // load maze from cache
            var maze = LoadMaze(); // TODO: if maze is null return error message

            // load player from cache
            var player = playerSavingService.LoadCurrentPlayerWithState(); // TODO: handle null


            var direction = player.Move(maze.CurrentPosition);
            var cell = maze.Move(direction);

            // save state
            SaveMaze(maze);
            playerSavingService.SaveCurrentPlayerWithState(player);

            // return new position, isSolved (current position == end)
            var json = JsonConvert.SerializeObject(cell);

            Thread.Sleep(60);
            return Content(json, "application/json");
        }

        public ContentResult ChoosePlayer(string team, string playerName)
        {
            var player = playerSavingService.LoadPlayer(team, playerName);
            playerSavingService.SaveCurrentPlayerWithState(player);

            return Content("Done");
        }

        #endregion

        #region Methods


        private static void SaveMaze(IMaze maze)
        {
            System.Web.HttpContext.Current.Session["maze"] = maze;
        }

        private static void SaveMazeJson(string json)
        {
            var cache = MemoryCache.Default;
            cache.Set("mazeJson", json, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
        }

        private static IMaze LoadMaze()
        {
            return (IMaze)System.Web.HttpContext.Current.Session["maze"];
        }

        private static string LoadMazeJson()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains("mazeJson"))
            {
                return (string)cache.Get("mazeJson");
            }
            return "";
        }

        #endregion

    }
}
