using Minesweeper.Models;

namespace Minesweeper;

public class Game
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime? StartTime { get; private set; } = null;
    public DateTime? EndTime { get; private set; } = null;
    public IPlayer Player { get; set; }

    private bool IsFirstTileRevealed { get; set; } = false;
    private Board _board { get; set; }
    public object Board { get; set; }
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public Game(IBoardGenerator boardGenerator, IPlayer player, int rows, int columns)
    {
        _board = boardGenerator.GenerateBoard();
        Player = player;
        Rows = rows;
        Columns = columns;
    }
    public void RevealTile_FirstTile_ShouldStartTimer()
    {
        StartTime = DateTime.Now;
    }

    public void RevealTile(int row, int column)
    {
        if (IsFirstTileRevealed == false)
        {
            RevealTile_FirstTile_ShouldStartTimer();
            IsFirstTileRevealed = true;
        }
        if (_board.Tiles[row, column].IsMine)
        {
            EndTime = DateTime.Now;
            throw new MineExplodedException();
        }
        _board.RevealTile(row, column);
    }

    public void FlagTile(int row, int column)
    {
        _board.FlagTile(row, column);
    }
    public void UnflagTile(int row, int column)
    {
        _board.UnflagTile(row, column);
    }
 
public double GetSecondsUsed()
{
        DateTime now = DateTime.Now;
        DateTime _startTime = StartTime ?? DateTime.MinValue;
        double seconds = (now - _startTime).TotalSeconds;
        return seconds;
}
}
