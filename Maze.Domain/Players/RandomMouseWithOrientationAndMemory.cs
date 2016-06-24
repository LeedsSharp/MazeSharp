using System;
using System.Collections.Generic;
using System.Linq;
using MazeSharp.Game;

namespace MazeSharp.Domain.Players
{
    /// <summary>
    /// A random direction is chosen every move.
    /// The algorithm remembers where it has been.
    /// The algorithm considers its orientation.
    /// The algorithm considers whether there is a wall in the chosen direction.
    /// </summary>
    public class RandomMouseWithOrientationAndMemory : IPlayer
    {
        #region Fields
        private readonly HashSet<ICell> visited;
        private Direction orientation;
        private readonly List<Direction> possibleDirections;
        #endregion

        public RandomMouseWithOrientationAndMemory()
        {
            visited = new HashSet<ICell>();
            possibleDirections = new List<Direction>();
        }

        public Direction Move(ICell cell)
        {
            // Remember current location
            visited.Add(cell);

            GetPossibleDirections(cell);

            // Try to continue in the same direction as previous move
            return GoOrientation(cell);
        }

        private void GetPossibleDirections(ICell cell)
        {
            possibleDirections.Clear();

            if (!cell.HasNorthWall && !HasNorthBeenVisited(cell))
            {
                possibleDirections.Add(Direction.North);
            }
            if (!cell.HasEastWall && !HasEastBeenVisited(cell))
            {
                possibleDirections.Add(Direction.East);
            }
            if (!cell.HasSouthWall && !HasSouthBeenVisited(cell))
            {
                possibleDirections.Add(Direction.South);
            }
            if (!cell.HasWestWall && !HasWestBeenVisited(cell))
            {
                possibleDirections.Add(Direction.West);
            }
        }

        private void GetPossibleDirectionsWithVisited(ICell cell)
        {
            possibleDirections.Clear();

            if (!cell.HasNorthWall)
            {
                possibleDirections.Add(Direction.North);
            }
            if (!cell.HasEastWall)
            {
                possibleDirections.Add(Direction.East);
            }
            if (!cell.HasSouthWall)
            {
                possibleDirections.Add(Direction.South);
            }
            if (!cell.HasWestWall)
            {
                possibleDirections.Add(Direction.West);
            }
        }

        private bool HasWestBeenVisited(ICell currentCell)
        {
            return visited.Any(cell => cell.X == currentCell.X - 1 && cell.Y == currentCell.Y);
        }

        private bool HasSouthBeenVisited(ICell currentCell)
        {
            return visited.Any(cell => cell.X == currentCell.X && cell.Y == currentCell.Y + 1);
        }

        private bool HasEastBeenVisited(ICell currentCell)
        {
            return visited.Any(cell => cell.X == currentCell.X + 1 && cell.Y == currentCell.Y);
        }

        private bool HasNorthBeenVisited(ICell currentCell)
        {
            return visited.Any(cell => cell.X == currentCell.X && cell.Y == currentCell.Y - 1);
        }

        private Direction GoOrientation(ICell currentCell)
        {
            switch (orientation)
            {
                case Direction.North:
                    return !currentCell.HasNorthWall ? Direction.North : GoRandomDirection(currentCell);
                case Direction.East:
                    return !currentCell.HasEastWall ? Direction.East : GoRandomDirection(currentCell);
                case Direction.South:
                    return !currentCell.HasSouthWall ? Direction.South : GoRandomDirection(currentCell);
                default:
                    return !currentCell.HasWestWall ? Direction.West : GoRandomDirection(currentCell);
            }
        }

        private Direction GoRandomDirection(ICell currentCell)
        {
            var randomiser = new Random(DateTime.Now.Millisecond);
            if (possibleDirections.Count == 0)
            {
                GetPossibleDirectionsWithVisited(currentCell);
            }
            var randomDirection = randomiser.Next(possibleDirections.Count);
            return GoDirection(randomDirection);

        }

        private Direction GoDirection(int randomDirection)
        {
            return possibleDirections[randomDirection];
        }
    }
}