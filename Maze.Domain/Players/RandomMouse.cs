using System;
using MazeSharp.Game;

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
        public Direction Move(ICell cell)
        {
            return GoRandomDirection(cell);
        }

        private static Direction GoRandomDirection(ICell cell)
        {
            var randomiser = new Random(DateTime.Now.Millisecond);
            var randomDirection = randomiser.Next(4);
            return GoDirection(cell, randomDirection);
        }

        private static Direction GoDirection(ICell cell, int randomDirection)
        {
            switch (randomDirection)
            {
                case 0:
                    return Direction.North;
                case 1:
                    return Direction.East;
                case 2:
                    return Direction.South;
                default:
                    return Direction.West;
            }
        }
    }
}