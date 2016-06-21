using System;
using System.Collections.Generic;
using System.Linq;
using MazeSharp.Game;

namespace MazeSharp.Domain.Players
{
    public class DepthFirstSearch : IPlayer
    {
        #region Fields
        private readonly HashSet<ICell> visited;
        private readonly Stack<ICell> stack;
        #endregion

        #region Constructors
        public DepthFirstSearch()
        {
            visited = new HashSet<ICell>();
            stack = new Stack<ICell>();
        } 
        #endregion

        public Direction Move(ICell cell)
        {
            /*
            Add current cell as visited
            Get possible directions
            Get all neighbouring cells that haven't been visited
            If found
                randomly choose one
                push it onto stack
                add to visited
                set as current position 
                return cell
            If not that means dead end
                pop next cell from stack
                if no more cells that means we're at the start again and maze is not solvable
                else
                add to visited (should already be there but HashSet means it doesn't matter if it is added again)
                set as current position
                return cell
            */

            var random = new Random(DateTime.Now.Millisecond);
            visited.Add(cell);
            stack.Push(cell);
            var possibleDirections = GetPossibleDirections(cell);
            if (possibleDirections.Any()) return possibleDirections[random.Next(possibleDirections.Count)];

            if (stack.Count <= 0) return Direction.None; // We're back at the start and the maze is not solvable

            // Backtrack
            var previousCell = stack.Pop();
            visited.Add(previousCell);
            return GetDirection(cell, previousCell);
        }

        private static Direction GetDirection(ICell fromCell, ICell toCell)
        {
            if (fromCell.X == toCell.X && fromCell.Y < toCell.Y)
            {
                return Direction.South;
            }

            if (fromCell.X == toCell.X && fromCell.Y > toCell.Y)
            {
                return Direction.North;
            }

            if (fromCell.X < toCell.X && fromCell.Y == toCell.Y)
            {
                return Direction.East;
            }
            return Direction.West;
        }

        private List<Direction> GetPossibleDirections(ICell cell)
        {
            var possibleDirections = new List<Direction>();

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

            return possibleDirections;
        }

        private bool HasWestBeenVisited(ICell cell)
        {
            return visited.Any(c => c.X == cell.X - 1 && c.Y == cell.Y);
        }

        private bool HasSouthBeenVisited(ICell cell)
        {
            return visited.Any(c => c.X == cell.X && c.Y == cell.Y + 1);
        }

        private bool HasEastBeenVisited(ICell cell)
        {
            return visited.Any(c => c.X == cell.X + 1 && c.Y == cell.Y);
        }

        private bool HasNorthBeenVisited(ICell cell)
        {
            return visited.Any(c => c.X == cell.X && c.Y == cell.Y - 1);
        }
    }
}