using System.Linq;
using System.Net.Configuration;
using MazeSharp.Domain;
using NUnit.Framework;

namespace MazeSharp.Tests
{
    [TestFixture]
    public class MazeTests
    {
        #region Setup

        [TestFixtureSetUp]
        public void SetUpOnce()
        {

        }

        [TestFixtureTearDown]
        public void TearDownOnce()
        {

        }

        [SetUp]
        public void SetupBeforeEveryTest()
        {

        }

        [TearDown]
        public void TearDownBeforeEveryTest()
        {

        }

        #endregion

        #region Tests

        [Test]
        public void Can_create_Maze_with_set_width_and_height()
        {
            var maze = new Maze(1,1);
            Assert.AreEqual(1, maze.Cells.GetLength(0));
            Assert.AreEqual(1, maze.Cells.GetLength(1));
        }

        [Test]
        public void New_Maze_is_not_solvable()
        {
            var maze = new Maze(5, 5);
            Assert.IsFalse(maze.IsSolvable);
        }

        [Test]
        public void Generate_creates_solvable_maze()
        {
            var maze = new Maze(5, 5);
            maze.Generate(1);
            Assert.IsTrue(maze.IsSolvable);
        }

        [Test]
        public void Generate_chooses_random_starting_Cell()
        {
            var maze = new Maze(5, 5);
            maze.Generate(1);
            var startingCell1 = maze.Start;
            maze.Generate(2);
            var startingCell2 = maze.Start;

            Assert.AreNotEqual(startingCell1.X, startingCell2.X);
            Assert.AreNotEqual(startingCell1.Y, startingCell2.Y);
        }

        [Test]
        public void GetNeighbours_returns_2_Cells_for_corner_Cell()
        {
            var maze = new Maze(5, 5);
            var cornerCell = maze.Cells[0, 0];
            var neighbours = maze.TestGetNeighbours(cornerCell);
            Assert.AreEqual(2, neighbours.Count);
        }

        [Test]
        public void Generate_creates_Maze_with_no_cells_having_all_walls_intakt()
        {
            var maze = new Maze(5, 5);
            var foundCellWithAllWallsIntakt = false;
            maze.Generate(1);
           
            for (var x = 0; x < 5; x++)
            {
                if (foundCellWithAllWallsIntakt)
                {
                    break;
                }
                for (var y = 0; y < 5; y++)
                {
                    if (!maze.Cells[x, y].AreAllWallsIntact) continue;
                    foundCellWithAllWallsIntakt = true;
                    break;
                }
            }
            Assert.IsFalse(foundCellWithAllWallsIntakt);
        }

        [Test]
        public void KnockWall_removes_north_wall_if_neighbour_is_north()
        {
            var current = new Cell(0, 1);
            var neighbour = new Cell(0, 0);
            var maze = new Maze(5, 5);
            maze.TestKnockWall(current, neighbour);

            Assert.IsFalse(current.AreAllWallsIntact, "Current has still all walls intact.");
            Assert.IsFalse(neighbour.AreAllWallsIntact, "Neighbour has still all walls intact.");
            Assert.IsFalse(current.HasNorthWall, "Current still has north wall.");
            Assert.IsFalse(neighbour.HasSouthWall, "Neighbour still has south wall.");
        }

        [Test]
        public void KnockWall_removes_south_wall_if_neighbour_is_south()
        {
            var current = new Cell(0, 1);
            var neighbour = new Cell(0, 2);
            var maze = new Maze(5, 5);
            maze.TestKnockWall(current, neighbour);

            Assert.IsFalse(current.AreAllWallsIntact, "Current has still all walls intact.");
            Assert.IsFalse(neighbour.AreAllWallsIntact, "Neighbour has still all walls intact.");
            Assert.IsFalse(current.HasSouthWall, "Current still has south wall.");
            Assert.IsFalse(neighbour.HasNorthWall, "Neighbour still has north wall.");
        }

        [Test]
        public void KnockWall_removes_east_wall_if_neighbour_is_east()
        {
            var current = new Cell(0, 0);
            var neighbour = new Cell(1, 0);
            var maze = new Maze(5, 5);
            maze.TestKnockWall(current, neighbour);

            Assert.IsFalse(current.AreAllWallsIntact, "Current has still all walls intact.");
            Assert.IsFalse(neighbour.AreAllWallsIntact, "Neighbour has still all walls intact.");
            Assert.IsFalse(current.HasEastWall, "Current still has east wall.");
            Assert.IsFalse(neighbour.HasWestWall, "Neighbour still has west wall.");
        }

        [Test]
        public void KnockWall_removes_west_wall_if_neighbour_is_west()
        {
            var current = new Cell(1, 0);
            var neighbour = new Cell(0, 0);
            var maze = new Maze(5, 5);
            maze.TestKnockWall(current, neighbour);

            Assert.IsFalse(current.AreAllWallsIntact, "Current has still all walls intact.");
            Assert.IsFalse(neighbour.AreAllWallsIntact, "Neighbour has still all walls intact.");
            Assert.IsFalse(current.HasWestWall, "Current still has west wall.");
            Assert.IsFalse(neighbour.HasEastWall, "Neighbour still has east wall.");
        }
        #endregion
    }
}