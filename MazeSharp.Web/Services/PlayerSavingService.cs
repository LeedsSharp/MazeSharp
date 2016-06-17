using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace MazeSharp.Web.Services
{
    public class PlayerSavingService<T> : IPlayerSavingService<T>
    {
        private readonly object _teamListSaveLock = new object();
        private readonly object _playerListSaveLock = new object();
        private readonly MemoryCache _cache;

        public PlayerSavingService()
        {
            _cache = MemoryCache.Default;
        }

        public void SavePlayer(string team, string playerName, T player)
        {
            SaveTeamName(team);
            SavePlayerName(team, playerName);
            var playerKey = $"{team}.{playerName}";
            _cache.Set(playerKey, player, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(1) });
        }

        public T LoadPlayer(string team, string playerName)
        {
            var playerKey = $"{team}.{playerName}";
            if (_cache.Contains(playerKey))
            {
                return (T)_cache.Get(playerKey);
            }
            return default(T);
        }

        public IList<string> GetTeams()
        {
            var key = KeyForTeamList();
            return _cache[key] as IList<string>;
        }

        public IList<string> GetPlayerNamesForTeam(string teamName)
        {
            var key = KeyForPlayerList(teamName);
            return _cache[key] as IList<string>;
        }

        public void SaveCurrentPlayerWithState(T player)
        {
            HttpContext.Current.Session["player"] = player;
        }

        public T LoadCurrentPlayerWithState()
        {
            return (T)HttpContext.Current.Session["player"];
        }
        private void AddToListInCache(string listKey, string listItem, object listLock)
        {
            lock (listLock)
            {
                var list = (IList<string>)_cache[listKey] ?? new List<string>();
                if (list.Contains(listItem)) return;

                list.Add(listItem);
                _cache[listKey] = list.OrderBy(s => s).ToList();
            }
        }

        private static string KeyForPlayerList(string teamName)
        {
            return $"..{teamName}..Players";;
        }

        private static string KeyForTeamList()
        {
            return "..teams";
        }

        private void SavePlayerName(string teamName, string playerName)
        {
            var key = KeyForPlayerList(teamName);
            AddToListInCache(key, playerName, _playerListSaveLock);
        }

        private void SaveTeamName(string teamName)
        {
            AddToListInCache(KeyForTeamList(), teamName, _teamListSaveLock);
        }

        
    }
}