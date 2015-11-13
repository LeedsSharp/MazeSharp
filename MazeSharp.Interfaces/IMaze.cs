namespace MazeSharp.Interfaces
{
    public interface IMaze
    {
        ICell CurrentPosition { get; set; }
        ICell GoNorth();
        ICell GoEast();
        ICell GoSouth();
        ICell GoWest();
    }
}