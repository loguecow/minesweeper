using Minesweeper.Models;

namespace Minesweeper.Implementation;

public class RandomBoardGenerator : IBoardGenerator
{
        private readonly int _rows;
        private readonly int _columns;
        private readonly int _mines;
    public RandomBoardGenerator(int rows, int columns, int mines)
    {
        _rows = rows;
        _columns = columns;
        _mines = mines;
    }

    public RandomBoardGenerator(Level level)
    {
        switch (level)
        {
                case Level.Beginner:
                    _rows = 9;
                    _columns = 9;
                    _mines = 10;
                    break;
                case Level.Intermediate:
                    _rows = 16;
                    _columns = 16;
                    _mines = 30;
                    break;
                case Level.Expert:
                    _rows = 30;
                    _columns = 30;
                    _mines = 90;
                    break;
        }
        
    }

public Board GenerateBoard()
{
    var board = new Board(_rows, _columns,_mines);
    var random = new Random();

    for (int i = 0; i < _rows; i++)
    {
        for (int j = 0; j < _columns; j++)
        {
            board.Tiles[i, j] = new Tile(false);
        }
    }

    for (int i = 0; i < _mines; i++)
    {
        int row, column;
        do
        {
            row = random.Next(_rows);
            column = random.Next(_columns);
        } while (board.Tiles[row, column].IsMine);

        board.PlaceMine(row, column);
    }

    return board;
}
}
