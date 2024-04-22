
namespace Minesweeper.Api.Models;

public class GameResponse
{
    public Guid GameId { get; set; }

    public Board Board { get; set; } = new Board();

    public bool MineExploded { get; set; }
    public bool GameWon { get; set; }
}