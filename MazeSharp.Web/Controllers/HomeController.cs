using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MazeSharp.Domain;
using MazeSharp.Domain.Players;
using MazeSharp.Interfaces;
using MazeSharp.Web.ViewModels;
using Newtonsoft.Json;

namespace MazeSharp.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Actions
        public ActionResult Index(string message)
        {
            var viewModel = new HomeIndexViewModel
            {
                Title = "Maze Sharp!",
                Message = message,
                Player = LoadPlayerName(),
                MazeJson = LoadMazeJson()
            };
            return View(viewModel);
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

        [HttpPost]
        public ActionResult UploadPlayer(HttpPostedFileBase dll)
        {
            var message = "Could not load assembly";

            // Load player dll
            if (dll.ContentLength > 0)
            {
                var fileName = Path.GetFileName(dll.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                dll.SaveAs(path);
                var playerAssembly = Assembly.LoadFile(path);
                foreach (var type in playerAssembly.GetTypes().OrderBy(t => t.Name))
                {
                    if (type.GetInterface("IPlayer") != null)
                    {
                        var player = Activator.CreateInstance(type) as IPlayer;
                        SavePlayer((IPlayer)player);
                        SavePlayerName(type.Name);
                        message = type.Name + " uploaded.";
                        return RedirectToAction("Index", new { message });
                    }
                }
                message = "No player implementing IPlayer found.";
                return RedirectToAction("Index", new { message });

            }

            return RedirectToAction("Index", new {message});
        }

        /// <summary>
        /// Load external dll from form. 
        /// Assume dll implements IPlayer interface.
        /// 
        /// </summary>
        /// <returns></returns>
        public ContentResult Move(int currentX, int currentY)
        {
            // load maze from cache
            var maze = LoadMaze(); // TODO: if maze is null return error message

            // load player from cache
            var player = LoadPlayer(); // TODO: handle null

            // maze.Solve(player)
            var cell = player.Move(maze);

            // save state
            SaveMaze(maze);
            SavePlayer(player);

            // return new position, isSolved (current position == end)
            var json = JsonConvert.SerializeObject(cell);
            //Thread.Sleep(5);
            return Content(json, "application/json");
        }
        #endregion

        #region Methods


        private static void SaveMaze(IMaze maze)
        {
            var cache = MemoryCache.Default;
            cache.Set("maze", maze, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
        }

        private static void SaveMazeJson(string json)
        {
            var cache = MemoryCache.Default;
            cache.Set("mazeJson", json, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
        }

        private static IMaze LoadMaze()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains("maze"))
            {
                return (IMaze)cache.Get("maze");
            }
            return null;
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

        private static void SavePlayer(IPlayer player)
        {
            var cache = MemoryCache.Default;
            cache.Set("player", player, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
        }

        private void SavePlayerName(string name)
        {
            var cache = MemoryCache.Default;
            cache.Set("playerName", name, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
        }

        private static IPlayer LoadPlayer()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains("player"))
            {
                return (IPlayer)cache.Get("player");
            }
            return new DepthFirstSearch();
        }

        private static string LoadPlayerName()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains("playerName"))
            {
                return (string)cache.Get("playerName");
            }
            return "LS#er";
        }
        #endregion

    }
}
