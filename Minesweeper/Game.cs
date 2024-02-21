using Minesweeper.Models;

namespace Minesweeper;

public class Game
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime? StartTime { get; private set; } = null;
    public DateTime? EndTime { get; private set; } = null;
    public IPlayer Player { get; set; }

    private bool IsFirstTileRevealed { get; set; } = false;

    public Game(IBoardGenerator boardGenerator, IPlayer player)
    {
        boardGenerator.GenerateBoard();
        Player = player;
    }
    public void RevealTile_FirstTile_ShouldStartTimer()
    {
        StartTime = DateTime.Now;
    }

    public void RevealTile(int row, int column)
    {
       
    }

    public void FlagTile(int row, int column)
    {
        throw new NotImplementedException();
    }

    public double GetSecondsUsed()
    {
        throw new NotImplementedException();
    }
}
