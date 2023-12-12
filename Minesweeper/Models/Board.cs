namespace Minesweeper.Models;

/// <summary>
/// This class represents a Minesweeper board.
/// </summary>
public class Board
{
    public Board() {}

    public Board(int columns, int rows, int mines)
    {
        Columns = columns;
        Rows = rows;
        Mines = mines;
        Tiles = new Tile[rows, columns];
    }
    
    public Tile[,] Tiles { get; private set; } = new Tile[0, 0];
    public int Mines { get; private set; } = 0;
    public int Columns { get; private set; } = 0;
    public int Rows { get; private set; } = 0;

    public void FlagTile(int column, int row)
    {
        throw new NotImplementedException();
    }

    public void UnflagTile(int column, int row)
    {
        throw new NotImplementedException();
    }

    public void RevealTile(int column, int row)
    {
        throw new NotImplementedException();
    }
}
