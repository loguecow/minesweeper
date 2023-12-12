using Minesweeper.Models;

namespace Minesweeper.Implementation;

public class StaticBoardGenerator : IBoardGenerator
{
    /// <summary>
    /// Returns a board based on a board definition string.
    /// The string defines the board as a grid of characters
    /// where 'm' represents a mine, '.' represents a blank tile.
    /// Each row is separated by a comma. Each row should have 
    /// equal amount of columns.
    /// </summary>
    /// <param name="boardDefinition">A string that defines the board.</param>
    /// <example>.......,..mm...,......m,m...m..,..m.m..,.mmm...,.......</example>
    public StaticBoardGenerator(string boardDefinition) 
    {
        // TODO: Implement this constructor
    }
    public Board GenerateBoard()
    {
        //TODO: Implement this method

        return new Board(0, 0, 0);
    }
}
