using System.Collections.Generic;

namespace MazeSharp.Web.Services
{
    public interface IPlayerSavingService<T>
    {
        T LoadPlayer(string team, string playerName);
        T LoadCurrentPlayerWithState();
        void SaveCurrentPlayerWithState(T player);
        void SavePlayer(string team, string playerName, T player);

        IList<string> GetTeams();
        IList<string> GetPlayerNamesForTeam(string teamName);
    }
}