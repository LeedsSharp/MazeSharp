using System;
using System.Collections.Generic;
using System.Linq;
using MazeSharp.Interfaces;

namespace MazeSharp.Players
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

        public ICell Move(IMaze maze)
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
            if (maze.CurrentPosition.IsStart)
            {
                visited.Add(maze.CurrentPosition);
                stack.Push(maze.CurrentPosition);
            }
            var possibleDirections = GetPossibleDirections(maze);
            if (possibleDirections.Any())
            {
                var direction = possibleDirections[random.Next(possibleDirections.Count)];
                var newPosition = GoOrientation(maze, direction);
                stack.Push(newPosition);
                visited.Add(newPosition);
                maze.CurrentPosition = newPosition;
                return newPosition;
            }

            if (stack.Count > 0)
            {
                var previousPosition = stack.Pop();
                visited.Add(previousPosition);
                maze.CurrentPosition = previousPosition;
                return previousPosition;
            }

            return maze.CurrentPosition; // We're back at the start and the maze is not solvable
        }
        private static ICell GoOrientation(IMaze maze, Compass direction)
        {
            switch (direction)
            {
                case Compass.North:
                    return maze.GoNorth();
                case Compass.East:
                    return maze.GoEast();
                case Compass.South:
                    return maze.GoSouth();
                default:
                    return maze.GoWest();
            }
        }

        private List<Compass> GetPossibleDirections(IMaze maze)
        {
            var possibleDirections = new List<Compass>();

            if (!maze.CurrentPosition.HasNorthWall && !HasNorthBeenVisited(maze))
            {
                possibleDirections.Add(Compass.North);
            }
            if (!maze.CurrentPosition.HasEastWall && !HasEastBeenVisited(maze))
            {
                possibleDirections.Add(Compass.East);
            }
            if (!maze.CurrentPosition.HasSouthWall && !HasSouthBeenVisited(maze))
            {
                possibleDirections.Add(Compass.South);
            }
            if (!maze.CurrentPosition.HasWestWall && !HasWestBeenVisited(maze))
            {
                possibleDirections.Add(Compass.West);
            }

            return possibleDirections;
        }

        private bool HasWestBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == maze.CurrentPosition.X - 1 && cell.Y == maze.CurrentPosition.Y);
        }

        private bool HasSouthBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == maze.CurrentPosition.X && cell.Y == maze.CurrentPosition.Y + 1);
        }

        private bool HasEastBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == maze.CurrentPosition.X + 1 && cell.Y == maze.CurrentPosition.Y);
        }

        private bool HasNorthBeenVisited(IMaze maze)
        {
            return visited.Any(cell => cell.X == maze.CurrentPosition.X && cell.Y == maze.CurrentPosition.Y - 1);
        }
    }
}