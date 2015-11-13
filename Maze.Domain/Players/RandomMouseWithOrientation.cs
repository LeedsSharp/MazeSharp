using System;
using System.Collections.Generic;
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
    public class RandomMouseWithOrientation : IPlayer
    {
        #region Fields
        private Compass orientation;
        #endregion

        #region Implementation of IPlayer
        public ICell Move(IMaze maze)
        {
            // Try to continue in the same direction as previous move
            return GoOrientation(maze);
        }
        #endregion

        #region Methods
        private ICell GoOrientation(IMaze maze)
        {
            switch (orientation)
            {
                case Compass.North:
                    return !maze.CurrentPosition.HasNorthWall ? maze.GoNorth() : GoRandomDirection(maze);
                case Compass.East:
                    return !maze.CurrentPosition.HasEastWall ? maze.GoEast() : GoRandomDirection(maze);
                case Compass.South:
                    return !maze.CurrentPosition.HasSouthWall ? maze.GoSouth() : GoRandomDirection(maze);
                default:
                    return !maze.CurrentPosition.HasWestWall ? maze.GoWest() : GoRandomDirection(maze);
            }
        }

        private ICell GoRandomDirection(IMaze maze)
        {
            var randomiser = new Random(DateTime.Now.Millisecond);
            var randomDirection = randomiser.Next(4);
            return GoDirection(maze, randomDirection);
        }

        private ICell GoDirection(IMaze maze, int randomDirection)
        {
            switch (randomDirection)
            {
                case 0:
                    orientation = Compass.North;
                    return maze.GoNorth();
                case 1:
                    orientation = Compass.East;
                    return maze.GoEast();
                case 2:
                    orientation = Compass.South;
                    return maze.GoSouth();
                default:
                    orientation = Compass.West;
                    return maze.GoWest();
            }
        } 
        #endregion
    }
}