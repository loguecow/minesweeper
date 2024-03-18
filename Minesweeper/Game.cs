using Minesweeper.Models;

namespace Minesweeper;

public class Game
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime? StartTime { get; private set; } = null;
    public DateTime? EndTime { get; private set; } = null;
    public IPlayer Player { get; set; }
    public Board Board { get; set; }

    public Game(IBoardGenerator boardGenerator, IPlayer player)
    {
        throw new NotImplementedException();
    }

    public void RevealTile(int row, int column)
    {
        throw new NotImplementedException();
    }

    public void FlagTile(int row, int column)
    {
        throw new NotImplementedException();
    }

    public double GetSecondsUsed()
    {
        throw new NotImplementedException();
    }

    public void UnflagTile(int row, int column)
    {
        throw new NotImplementedException();
    }
}
