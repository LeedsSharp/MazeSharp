using MazeSharp.Interfaces;

namespace MazeSharp.Players
{
    public class StandStillPlayer : IPlayer
    {
        public ICell Move(IMaze maze)
        {
            return maze.CurrentPosition;
        }
    }
}