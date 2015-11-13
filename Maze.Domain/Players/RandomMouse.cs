using System;
using MazeSharp.Interfaces;

namespace MazeSharp.Domain.Players
{
    /// <summary>
    /// This is the absolute dummest algorithm. 
    /// A random direction is chosen every move.
    /// The algorithm does not remember where it's been.
    /// The algorithm does not consider its orientation.
    /// The algorithm does not consider whether there is a wall in the chosen direction (= bumps it's head against it for the move).
    /// </summary>
    public class RandomMouse : IPlayer
    {
        public ICell Move(IMaze maze)
        {
            return GoRandomDirection(maze);
        }

        private static ICell GoRandomDirection(IMaze maze)
        {
            var randomiser = new Random(DateTime.Now.Millisecond);
            var randomDirection = randomiser.Next(4);
            return GoDirection(maze, randomDirection);
        }

        private static ICell GoDirection(IMaze maze, int randomDirection)
        {
            switch (randomDirection)
            {
                case 0:
                    return maze.GoNorth();
                case 1:
                    return maze.GoEast();
                case 2:
                    return maze.GoSouth();
                default:
                    return maze.GoWest();
            }
        }
    }
}