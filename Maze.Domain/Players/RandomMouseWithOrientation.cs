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
    public class RandomMouseWithOrientation : IPlayer
    {
        #region Fields
        private Direction orientation = Direction.None;
        #endregion

        #region Implementation of IPlayer
        public Direction Move(ICell cell)
        {
            // Try to continue in the same direction as previous move
            return GoOrientation(cell);
        }
        #endregion

        #region Methods
        private Direction GoOrientation(ICell cell)
        {
            switch (orientation)
            {
                case Direction.North:
                    return !cell.HasNorthWall ? Direction.North : GoRandomDirection(cell);
                case Direction.East:
                    return !cell.HasEastWall ? Direction.East : GoRandomDirection(cell);
                case Direction.South:
                    return !cell.HasSouthWall ? Direction.South : GoRandomDirection(cell);
                default:
                    return !cell.HasWestWall ? Direction.West : GoRandomDirection(cell);
            }
        }

        private Direction GoRandomDirection(ICell cell)
        {
            var randomiser = new Random(DateTime.Now.Millisecond);
            var randomDirection = randomiser.Next(4);
            return GoDirection(cell, randomDirection);
        }

        private Direction GoDirection(ICell cell, int randomDirection)
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
        #endregion
    }
}