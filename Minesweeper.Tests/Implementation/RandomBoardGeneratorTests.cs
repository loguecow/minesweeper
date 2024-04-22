using Minesweeper.Implementation;
using Minesweeper.Models;

namespace Minesweeper.Tests.Implementation;
public class RandomBoardGeneratorTests
{
    [Fact]
    public void GenerateBoard_Valid_ShouldGenerateBoard()
    {
        IBoardGenerator boardGenerator = new RandomBoardGenerator(8, 8, 10);
        
        Board board = boardGenerator.GenerateBoard();

        int mines = 0;
        foreach (var item in board.Tiles)
        {
            mines = item.IsMine ? mines + 1 : mines;
        }

        Assert.Equal(10, mines);
    }
}
