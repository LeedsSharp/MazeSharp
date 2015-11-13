using MazeSharp.Domain;
using NUnit.Framework;

namespace MazeSharp.Tests
{
    [TestFixture]
    public class CellTests
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
        public void AreAllWallsIntakt_returns_true_for_new_Cell()
        {
            var cell = new Cell(0,0);
            Assert.IsTrue(cell.AreAllWallsIntact);
        }
        
        [Test]
        public void IsStart_returns_false_for_new_Cell()
        {
            var cell = new Cell(0,0);
            Assert.IsFalse(cell.IsStart);
        }
        #endregion
    }
}