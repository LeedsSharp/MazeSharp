using System.Web.Script.Serialization;
using MazeSharp.Interfaces;

namespace MazeSharp.Domain
{
    public class Cell : ICell
    {
        #region Fields

        
        #endregion

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Wall = new[] { true, true, true, true }; // N, E, S, W
            IsStart = false;
        }

        #region Properties

        public int X { get; set; }
        public int Y { get; set; }
        public bool HasNorthWall
        {
            get { return Wall[0]; }
        }

        public bool HasEastWall
        {
            get { return Wall[1]; }
        }

        public bool HasSouthWall
        {
            get { return Wall[2]; }
        }

        public bool HasWestWall
        {
            get { return Wall[3]; }
        }

        public bool IsStart { get; set; }
        public bool IsExit { get; set; }
        public bool IsEnd { get; set; }

        public bool[] Wall { get; set; }

        [ScriptIgnore]
        public bool AreAllWallsIntact
        {
            get
            {
                for (var w = 0; w < 4; w++)
                {
                    if (!Wall[w])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool IsVisited { get; set; }

        #endregion
    }
}