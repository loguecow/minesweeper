using FluentAssertions;
using Minesweeper.Implementation;
using Minesweeper.Models;

namespace Minesweeper.Tests.Implementation;

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
        board.Mines.Should().Be(10);
    }

    [Fact]
    public void GenerateBoard_ValidBoardDefinition_ShouldReturnValidBoardWithMines()
    {
        string boardDefinition = ".......,..mm...,......m,m...m..,..m.m..,.mmm...,.......";

        IBoardGenerator boardGenerator = new StaticBoardGenerator(boardDefinition);
        Board board = boardGenerator.GenerateBoard();

        board.Tiles[1, 3].IsMine.Should().BeTrue();
        board.Tiles[0, 0].IsMine.Should().BeFalse();
        board.Mines.Should().Be(10);
    }

    [Fact]
    public void GenerateBoard_InvalidMissingColumn_ShouldThrowException()
    {
        string boardDefinition = ".......,..mm...,......m,m...m..,..m.m..,.mmm...,......,";

        Action newStaticBoardGenerator = () => new StaticBoardGenerator(boardDefinition);

        newStaticBoardGenerator.Should().Throw<ArgumentException>().WithMessage("*must have equal amount of columns*");
    }

    [Fact]
    public void GenerateBoard_InvalidNoMines_ShouldThrowException()
    {
        string boardDefinition = ".......,.......,.......,.......,.......,.......,.......";

        Action newStaticBoardGenerator = () => new StaticBoardGenerator(boardDefinition);

        newStaticBoardGenerator.Should().Throw<ArgumentException>().WithMessage("*must have at least one mine*");
    }

    [Fact]
    public void GenerateBoard_InvalidNoBoardDefinition_ShouldThrowException()
    {
        Action newStaticBoardGenerator = () => new StaticBoardGenerator("");

        newStaticBoardGenerator.Should().Throw<ArgumentException>().WithMessage("*cannot be null or empty*");
    }

    [Fact]
    public void GenerateBoard_InvalidTooFewRows_ShouldThrowException()
    {
        string boardDefinition = ".......,..mm...";

        Action newStaticBoardGenerator = () => new StaticBoardGenerator(boardDefinition);

        newStaticBoardGenerator.Should().Throw<ArgumentException>().WithMessage("*must have at least three rows*");
    }

    [Fact]
    public void GenerateBoard_InvalidTooFewColumns_ShouldThrowException()
    {
        string boardDefinition = "..,.m,..";

        Action newStaticBoardGenerator = () => new StaticBoardGenerator(boardDefinition);

        newStaticBoardGenerator.Should().Throw<ArgumentException>().WithMessage("*must have at least three columns*");
    }
}