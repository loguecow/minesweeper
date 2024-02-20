namespace Minesweeper;

public class Player : IPlayer
{
    public Player()
    {}

    public Guid Id { get; }

    public string Name { get; set; }
}
