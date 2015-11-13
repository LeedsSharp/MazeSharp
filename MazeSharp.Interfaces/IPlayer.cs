namespace MazeSharp.Interfaces
{
    public interface IPlayer
    {
        ICell Move(IMaze maze);
    }
}