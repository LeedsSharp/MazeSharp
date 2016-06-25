namespace MazeSharp.Game
{
    public interface ICell
    {
        int X { get; set; }
        int Y { get; set; }
        bool HasNorthWall { get; }
        bool HasEastWall { get; }
        bool HasSouthWall { get; }
        bool HasWestWall { get; }

        bool IsStart { get; set; }
        bool IsExit { get; set; }
    }
}