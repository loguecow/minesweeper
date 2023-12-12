using FluentAssertions;
using Minesweeper.Implementation;
using Minesweeper.Models;

namespace Minesweeper.Tests
{
    public class StaticBoardGeneratorTests
    {
        [Fact]
        public void GenerateBoard_ValidBoardDefinition_ShouldReturnValidBoard()
        {
            // Arrange
            string boardDefinition = 
                ".......," +
                "..mm...," +
                "......m," +
                "m...m..," +
                "..m.m..," +
                ".mmm...," +
                ".......";

            // Act
            IBoardGenerator boardGenerator = new StaticBoardGenerator(boardDefinition);
            Board board = boardGenerator.GenerateBoard();

            // Assert
            board.Columns.Should().Be(7);
            board.Rows.Should().Be(7);
        }

        [Fact]
        public void GenerateBoard_ValidBoardDefinition_ShouldReturnValidBoardWithMines()
        {
            // Arrange
            string boardDefinition = ".......,..mm...,......m,m...m..,..m.m..,.mmm...,";
            // Act
            IBoardGenerator boardGenerator = new StaticBoardGenerator(boardDefinition);
            Board board = boardGenerator.GenerateBoard();

            // Assert
            board.Tiles[2,3].IsMine.Should().BeTrue();
            board.Tiles[0,0].IsMine.Should().BeFalse();
            board.Mines.Should().Be(10);
        }

        [Fact]
        public void GenerateBoard_InvalidMissingColumn_ShouldThrowException()
        {
            // Arrange
            string boardDefinition = ".......,..mm...,......m,m...m..,..m.m..,.mmm...,......,";
         
            // Act
            IBoardGenerator boardGenerator = new StaticBoardGenerator(boardDefinition);
            
            // Assert
            boardGenerator.Invoking(bg => bg.GenerateBoard()).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GenerateBoard_InvalidNoMines_ShouldThrowException()
        {
            // Arrange
            string boardDefinition = ".......,.......,.......,.......,.......,.......,.......";
         
            // Act
            IBoardGenerator boardGenerator = new StaticBoardGenerator(boardDefinition);
            
            // Assert
            boardGenerator.Invoking(bg => bg.GenerateBoard()).Should().Throw<ArgumentException>();
        }
    }
}