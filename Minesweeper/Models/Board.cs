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

    public bool GameWon { get; private set; } = false;
    public Tile[,] Tiles { get; private set; } = new Tile[0, 0];
    public int Mines { get; private set; } = 0;
    public int Columns { get; private set; } = 0;
    public int Rows { get; private set; } = 0;
    public int CorrectlyFlaggedTiles { get; private set; } = 0;

    public void FlagTile(int column, int row)
    {
        //make IsFlagged true
        //if IsMine is true, increment CorrectlyFlaggedTiles
        Tiles[column, row].IsFlagged = true;
        if (Tiles[column,row].IsFlagged == true && Tiles[column,row].IsMine == true)
        {
            CorrectlyFlaggedTiles++;
        }
        if (CorrectlyFlaggedTiles == Mines)
        {
            GameWon = true;
        }
    }

    public void UnflagTile(int column, int row)
    {
        if (Tiles[column, row].IsFlagged == true && Tiles[column, row].IsMine == true)
        {
            CorrectlyFlaggedTiles--;
        }
        Tiles[column, row].IsFlagged = false;
    }

    public void RevealTile(int column, int row)
    {
        throw new NotImplementedException();
    }
}
