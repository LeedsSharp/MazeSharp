using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using MazeSharp.Interfaces;

namespace MazeSharp.Domain
{
    public class Maze : IMaze
    {
        #region Fields

        #endregion

        #region Constructors
        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            VisitedCells = 0;
            Cells = new Cell[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    Cells[x, y] = new Cell(x, y);
                }
            }
        }
        #endregion

        #region Properties

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Cell[,] Cells { get; set; }

        public Cell Start { get; set; }

        public Cell End { get; set; }

        /// <summary>
        /// Store current position of player when solving.
        /// </summary>
        public ICell CurrentPosition { get; set; }

        [ScriptIgnore]
        public bool IsSolvable
        {
            get
            {
                foreach (var cell in Cells)
                {
                    if (cell == null || cell.AreAllWallsIntact)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public int VisitedCells { get; private set; }

        #endregion

        #region Methods
        public void Generate(int seed)
        {
            var stack = new Stack<Cell>();
            var randomiser = new Random(seed);
            var current = Cells[Width - 1, randomiser.Next(Height)];
            End = current; // we are backtracking from the exit
            current.IsEnd = true;
            stack.Push(current);

            while (stack.Count > 0)
            {
                current.IsVisited = true;
                Console.WriteLine("Stack count is {0}.", stack.Count);

                // Get neighbouring cells that still have all their walls intakt
                var neighbours = GetNeighbours(current);
                Console.WriteLine("Found {0} neighbours.", neighbours.Count);
                if (neighbours.Count > 0)
                {
                    // Select a random neighbour
                    var neighbour = neighbours[randomiser.Next(neighbours.Count)];

                    Console.WriteLine("Randomly selected neighbour X: {0} Y:{1}.", neighbour.X, neighbour.Y);
                    KnockWall(current, neighbour);

                    Console.WriteLine("Pushing X: {0} Y:{1} onto stack.", current.X, current.Y);
                    stack.Push(current);
                    current = neighbour;
                }
                else
                {
                    Console.WriteLine("Popping stack.");
                    current = stack.Pop();
                }
            }


            Start = Cells[0, randomiser.Next(Height)];
            Start.Wall[3] = false;
            Start.IsStart = true;

            CurrentPosition = Start;

            End.Wall[1] = false;
            End.IsExit = true;

            if (IsPerfect) return;
            
            // remove random walls to make a non-perfect maze = more difficult
            for (var i = 0; i < 50; i++)
            {
                var randomCell = Cells[randomiser.Next(1, Width-1), randomiser.Next(1, Height-1)];
                randomCell.Wall[randomiser.Next(3)] = false;
            }
        }

        public bool IsPerfect { get; set; }

        public void Solve(IPlayer player)
        {
            // TODO
        }
        public ICell GoNorth()
        {
            if (!CurrentPosition.HasNorthWall && CurrentPosition.Y > 0) CurrentPosition = Cells[CurrentPosition.X, CurrentPosition.Y - 1];
            return CurrentPosition;
        }

        public ICell GoEast()
        {
            if (!CurrentPosition.HasEastWall && CurrentPosition.X < Width) CurrentPosition = Cells[CurrentPosition.X + 1, CurrentPosition.Y];
            return CurrentPosition;
        }

        public ICell GoSouth()
        {
            if (!CurrentPosition.HasSouthWall && CurrentPosition.Y < Height) CurrentPosition = Cells[CurrentPosition.X, CurrentPosition.Y + 1];
            return CurrentPosition;
        }

        public ICell GoWest()
        {
            if (!CurrentPosition.HasWestWall && CurrentPosition.X > 0) CurrentPosition = Cells[CurrentPosition.X - 1, CurrentPosition.Y];
            return CurrentPosition;
        }

        private void KnockWall(Cell current, Cell neighbour)
        {
            // TODO: Which wall leads to the neighbour?

            // Poor man's solution
            // Neighbour is South
            if (current.X == neighbour.X && current.Y < neighbour.Y)
            {
                current.Wall[(int)Direction.South] = false;
                neighbour.Wall[(int)Direction.North] = false;
                return;
            }
            // Neighbour is North
            if (current.X == neighbour.X && current.Y > neighbour.Y)
            {
                current.Wall[(int)Direction.North] = false;
                neighbour.Wall[(int)Direction.South] = false;
                return;

            }
            // Neighbour is East
            if (current.Y == neighbour.Y && current.X < neighbour.X)
            {
                current.Wall[(int)Direction.East] = false;
                neighbour.Wall[(int)Direction.West] = false;
                return;
            }

            // Neighbour is West
            current.Wall[(int)Direction.West] = false;
            neighbour.Wall[(int)Direction.East] = false;
        }

        private void ResetStart()
        {
            foreach (var cell in Cells)
            {
                cell.IsStart = false;
            }
        }

        private List<Cell> GetNeighbours(Cell cell)
        {
            var neighbours = new List<Cell>();

            // North
            var northX = cell.X;
            var northY = cell.Y + 1;

            if (northY < Height)
            {
                var northCell = Cells[northX, northY];
                if (!northCell.IsVisited)
                {
                    neighbours.Add(northCell);
                }
            }

            // East
            var eastX = cell.X + 1;
            var eastY = cell.Y;
            if (eastX < Width)
            {
                var eastCell = Cells[eastX, eastY];
                if (!eastCell.IsVisited)
                {
                    neighbours.Add(eastCell);
                }
            }

            // South
            var southX = cell.X;
            var southY = cell.Y - 1;
            if (southY >= 0)
            {
                var southCell = Cells[southX, southY];
                if (!southCell.IsVisited)
                {
                    neighbours.Add(southCell);
                }
            }

            // West
            var westX = cell.X - 1;
            var westY = cell.Y;
            if (westX >= 0)
            {
                var westCell = Cells[westX, westY];
                if (!westCell.IsVisited)
                {
                    neighbours.Add(westCell);
                }
            }

            return neighbours;
        }
        #endregion

        #region Test methods
        public List<Cell> TestGetNeighbours(Cell cell)
        {
            return GetNeighbours(cell);
        }

        public void TestKnockWall(Cell current, Cell neighbour)
        {
            KnockWall(current, neighbour);
        }
        #endregion

        public void Generate(int seed, bool isPerfect)
        {
            IsPerfect = isPerfect;
            Generate(seed);
        }
    }
}