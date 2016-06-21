namespace MazeSharp.Game
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