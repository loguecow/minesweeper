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
    public int IsreavealedTiles { get; private set; } = 0;

    public void FlagTile(int row, int column)
    {
        Tiles[row, column].IsFlagged = true;
        if (Tiles[row, column].IsFlagged == true && Tiles[row, column].IsMine == true)
        {
            CorrectlyFlaggedTiles++;
        }
        if (CorrectlyFlaggedTiles == Mines)
        {
            GameWon = true;
            MakeAllTiles(flag: true);
        }
    }

    public void UnflagTile(int row, int column)
    {
        if (Tiles[row, column].IsFlagged == true && Tiles[row, column].IsMine == true)
        {
            CorrectlyFlaggedTiles--;
        }
        Tiles[row, column].IsFlagged = false;
    }

    public void RevealTile(int row, int column)
    {

        if (Tiles[row,column].IsMine)
        {
            throw new MineExplodedException();
        }

        Tiles[row,column].IsRevealed = true;
        IsreavealedTiles++;
        
        OnlyRemainingMines();

        (int Row, int Column)[] adjacentTileReferences = GetAdjacentTileReferences(row, column);
        Tiles[row, column].AdjacentMines = CountAdjacentMines(adjacentTileReferences);
        
        if (Tiles[row,column].AdjacentMines == 0)
            RevealAdjacentTiles(adjacentTileReferences);

    }
    private void MakeAllTiles(bool flag)
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (flag)
                {
                    if (!Tiles[row, column].IsRevealed)
                    {
                    Tiles[row, column].IsRevealed = true;
                    }
                }
                else
                {
                    if (!Tiles[row, column].IsRevealed)
                    {
                        Tiles[row, column].IsFlagged = true;
                    }
                }
            }
        }
    }

    private int totalTiles()
    {
        return Rows * Columns;
    }
    private void OnlyRemainingMines()
    {
        int _totalTiles = totalTiles();
        int unrevealedTiles = _totalTiles - IsreavealedTiles;
        if (unrevealedTiles == Mines)
        {
            GameWon = true;
            MakeAllTiles(false);
        }
    }
    public void PlaceMine(int row, int column)
    {
        Tiles[row,column].IsMine = true;
    }

    private void RevealAdjacentTiles((int Row, int Column)[] adjacentTileReferences)
    {
    foreach ((int Row, int Column) tileReference in adjacentTileReferences)
    {
        if (tileReference.Row == -1 || tileReference.Column == -1)
            continue;

        if (!Tiles[tileReference.Row, tileReference.Column].IsRevealed)
            RevealTile(tileReference.Row, tileReference.Column);
    }
    }
    private int CountAdjacentMines((int Row, int Column)[] adjacentTileReferences)
    {
        int adjacentMines = 0;
        foreach ((int Row, int Column) tileReference in adjacentTileReferences)
        {
            if (tileReference.Row == -1 && tileReference.Column == -1)
                continue;

            if (Tiles[tileReference.Row, tileReference.Column].IsMine)
                adjacentMines++;
        }
        return adjacentMines;
    }

    private (int Row, int Column)[] GetAdjacentTileReferences(int row, int column)
    {
    (int row, int col) left = column == 0 ? (-1, -1) : (row, column - 1);
    (int row, int col) right = column == Columns - 1 ? (-1, -1) : (row, column + 1);
    (int row, int col) top = row == 0 ? (-1, -1) : (row - 1, column);
    (int row, int col) bottom = row == Rows - 1 ? (-1, -1) : (row + 1, column);
    (int row, int col) topLeft = row == 0 || column == 0 ? (-1, -1) : (row - 1, column - 1);
    (int row, int col) topRight = row == 0 || column == Columns - 1 ? (-1, -1) : (row - 1, column + 1);
    (int row, int col) bottomLeft = row == Rows - 1 || column == 0 ? (-1, -1) : (row + 1, column - 1);
    (int row, int col) bottomRight = row == Rows - 1 || column == Columns - 1 ? (-1, -1) : (row + 1, column + 1);

    return [left, right, top, bottom, topLeft, topRight, bottomLeft, bottomRight];
    }

}