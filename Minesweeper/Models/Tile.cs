namespace Minesweeper.Models;

public class Tile
{
    public Tile(bool isMine)
    {
        IsMine = isMine;
    }

    public bool IsMine { get; set; }
    public bool IsFlagged { get; set; } = false;
    public bool IsRevealed { get; set; } = false;
    public int AdjacentMines { get; set; } = 0;
}