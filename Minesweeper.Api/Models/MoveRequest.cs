namespace Minesweeper.Api.Models;

public class MoveRequest
{
    public int Row { get; set; }
    public int Column { get; set; }
    public Move Move { get; set; }
}