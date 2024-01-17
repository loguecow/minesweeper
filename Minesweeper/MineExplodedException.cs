namespace Minesweeper;

public class MineExplodedException : Exception
{
    public MineExplodedException()
    {
    }

    public MineExplodedException(string? message) : base(message)
    {
    }

    public MineExplodedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

}