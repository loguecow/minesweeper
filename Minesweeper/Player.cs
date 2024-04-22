namespace Minesweeper;

public class Player : IPlayer
{
    public Player()
    { Name = "Player";}
    public Player(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public string Name { get; set; }
}
