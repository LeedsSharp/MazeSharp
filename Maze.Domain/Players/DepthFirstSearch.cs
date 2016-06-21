using System;
using System.Collections.Generic;
using System.Linq;
using MazeSharp.Interfaces;

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
            if (cell.IsStart)
            {
                visited.Add(cell);
                stack.Push(cell);
            }
            var possibleDirections = GetPossibleDirections(cell);
            if (possibleDirections.Any())
            {
                var direction = possibleDirections[random.Next(possibleDirections.Count)];
                var newPosition = GoOrientation(cell, direction);
                stack.Push(newPosition);
                visited.Add(newPosition);
                cell = newPosition;
                return newPosition;
            }

            if (stack.Count > 0)
            {
                var previousPosition = stack.Pop();
                visited.Add(previousPosition);
                cell = previousPosition;
                return previousPosition;
            }

            return cell; // We're back at the start and the maze is not solvable
        }
        private static ICell GoOrientation(IMaze maze, Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return maze.GoNorth();
                case Direction.East:
                    return maze.GoEast();
                case Direction.South:
                    return maze.GoSouth();
                default:
                    return maze.GoWest();
            }
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

        private bool HasWestBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == cell.X - 1 && cell.Y == cell.Y);
        }

        private bool HasSouthBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == cell.X && cell.Y == cell.Y + 1);
        }

        private bool HasEastBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == cell.X + 1 && cell.Y == cell.Y);
        }

        private bool HasNorthBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == cell.X && cell.Y == cell.Y - 1);
        }
    }
}