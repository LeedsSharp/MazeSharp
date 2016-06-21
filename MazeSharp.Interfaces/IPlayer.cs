namespace MazeSharp.Game
{
    public interface IPlayer
    {
        Direction Move(ICell cell);
    }
}