using MazeSharp.Game;

namespace MazeSharp.Domain.Extensions
{
    public static class PlayerExtensions
    {
        public static string GetName(this IPlayer player)
        {
            return player?.GetType().Name;
        }

        public static string GetTeamName(this IPlayer player)
        {
            return player?.GetType().Namespace;
        }
    }
}