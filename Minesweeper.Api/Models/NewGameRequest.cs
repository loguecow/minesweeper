using Minesweeper.Models;

namespace Minesweeper.Api.Models;

public class NewGameRequest
{
    public Level Level { get; set; }

    public string? UserName { get; set; } = "Anonymous";

    public Guid UserId { get; set; }
}
