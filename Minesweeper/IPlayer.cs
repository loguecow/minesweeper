namespace Minesweeper;

public interface IPlayer
{
    Guid Id { get; }
    string Name { get; set; }
}