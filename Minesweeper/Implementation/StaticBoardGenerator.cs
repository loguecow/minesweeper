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
    private readonly string[] _rows;
    private readonly int _columns;
    private readonly int _minesCount;
    private readonly string _boardDefinition;
    public int minimumColumns = 3;
    public int minimumRows = 3;
    

    public StaticBoardGenerator(string boardDefinition) 
    {

        _boardDefinition = boardDefinition;
        // TODO: Implement this constructor
        _rows = _boardDefinition.Split(',');
        _minesCount = _boardDefinition.Count(c => c == 'm');

        

    }
    public Board GenerateBoard()
    {
    int rowsCount = _rows.Length;
    int columnsCount = _rows[0].Length;
    Board board = new Board(columnsCount, rowsCount, _minesCount);
    for (int i = 0; i < board.Rows; i++)
    {
        for (int j = 0; j < board.Columns; j++)
        {
            bool isMine = _rows[i][j] == 'm';

            Tile tile = new Tile(isMine);

            board.Tiles[i, j] = tile;
        }
    }

    return board;
    }
}
