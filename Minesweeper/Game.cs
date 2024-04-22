using Minesweeper.Models;

namespace Minesweeper;

public class Game
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime? StartTime { get; private set; } = null;
    public DateTime? EndTime { get; private set; } = null;
    public IPlayer Player { get; set; }

    private bool IsFirstTileRevealed { get; set; } = false;
    public Board _board { get; set; }
    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public Game(IBoardGenerator boardGenerator, IPlayer player)
    {
        _board = boardGenerator.GenerateBoard();
        Player = player;
        Rows = _board.Rows;
        Columns = _board.Columns;
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
            _board.Tiles[row, column].Exploded = true;

        for (int i = 0; i < _board.Rows; i++)
        {
            for (int j = 0; j < _board.Columns; j++)
            {
                if (_board.Tiles[i, j].IsMine)
                {
                    _board.Tiles[i, j].Exploded = true;
                }
            }
        }

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
