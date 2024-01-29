using FluentAssertions;
using Minesweeper.Implementation;
using Minesweeper.Models;

namespace Minesweeper.Tests.Models;
public class BoardTests
{
    private const string BOARD_DEFINITION = 
        "......m," + 
        "..mm...," +
        "......m," +
        "m...m..," +
        "..m....," +
        ".mmm...," +
        ".......";
    
    [Fact]
    public void FlagTile_NotFlagged_ShouldFlag()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.FlagTile(3, 3);

        board.Tiles[3, 3].IsFlagged.Should().BeTrue();
    }

    [Fact]
    public void FlagTile_Flagged_ShouldStillBeFlagged()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();
        board.FlagTile(3, 3);

        board.FlagTile(3, 3);

        board.Tiles[3, 3].IsFlagged.Should().BeTrue();
    }

    [Fact]
    public void FlagTile_Mine_ShouldCountUpCorrectlyFlaggedTiles()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.FlagTile(1, 2);

        board.CorrectlyFlaggedTiles.Should().Be(1);
    }

    [Fact]
    public void FlagTile_Mine_ShouldCountDownCorrectlyFlaggedTiles()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.FlagTile(1, 2);
        board.UnflagTile(1, 2);

        board.CorrectlyFlaggedTiles.Should().Be(0);
    }

    [Fact]
    public void FlagTile_AllFlagsCorrectlyFlagged_GameWon()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();
        
        board.FlagTile(0, 6);
        board.FlagTile(1, 2);
        board.FlagTile(1, 3);
        board.FlagTile(2, 6);
        board.FlagTile(3, 0);
        board.FlagTile(3, 4);
        board.FlagTile(4, 2);
        board.FlagTile(5, 1);
        board.FlagTile(5, 2);
        board.FlagTile(5, 3);

        board.GameWon.Should().BeTrue();
    }

    [Fact]
    public void UnFlagTile_Flagged_ShouldUnFlag()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();
        board.FlagTile(3, 3);

        board.UnflagTile(3, 3);

        board.Tiles[3, 3].IsFlagged.Should().BeFalse();
    }

    [Fact]
    public void UnFlagTile_NotFlagged_ShouldStillBeUnFlagged()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.UnflagTile(3, 3);

        board.Tiles[3, 3].IsFlagged.Should().BeFalse();
    }

    [Fact]
    public void RevealTile_Mine_ShouldExplode()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        Action revealTile = () => board.RevealTile(1, 2);

        revealTile.Should().Throw<MineExplodedException>();
    }

    [Fact]
    public void RevealTile_EmptyNotReveald_ShouldReveal()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.RevealTile(2, 2);

        board.Tiles[2, 2].IsRevealed.Should().BeTrue();
    }

    [Fact]
    public void RevealTile_EmptyHasAdjacentMines_ShouldHaveCorrectAdjacentMinesCount()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.RevealTile(2, 1);

        board.Tiles[2, 1].AdjacentMines.Should().Be(2);
    }

    [Fact]
    public void RevealTile_EmptyHasNoAdjacentMinesBorderTile_ShouldHaveCorrectAdjacentMinesCount()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();
        
        board.RevealTile(0, 0);

        board.Tiles[0, 0].AdjacentMines.Should().Be(0);
    }

    [Fact]
    public void RevealTile_EmptyHasNoAdjacentMines_ShouldHaveCorrectAdjacentMinesCount()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.RevealTile(5, 5);

        board.Tiles[5, 5].AdjacentMines.Should().Be(0);
    }

    [Fact]
    public void RevealTile_EmptyHasNoAdjacentMines_ShouldRevealAllTilesWithNoAdjacentMines()
    {
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        board.RevealTile(5, 5);

        board.Tiles[3, 6].AdjacentMines.Should().Be(1);
        board.Tiles[4, 4].AdjacentMines.Should().Be(2);
        board.Tiles[4, 5].AdjacentMines.Should().Be(1);
        board.Tiles[4, 6].AdjacentMines.Should().Be(0);
        board.Tiles[5, 4].AdjacentMines.Should().Be(1);
        board.Tiles[5, 5].AdjacentMines.Should().Be(0);
        board.Tiles[5, 6].AdjacentMines.Should().Be(0);
        board.Tiles[6, 4].AdjacentMines.Should().Be(1);
        board.Tiles[6, 5].AdjacentMines.Should().Be(0);
        board.Tiles[6, 6].AdjacentMines.Should().Be(0);
    }

    [Fact]
    public void RevealTile_EmptyHasAdjacentMine_ShouldOnlyRevealSelected()
    {
        // Arrange
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        // Act
        board.RevealTile(6, 0);

        // Assert
        int revealdTiles = 0;
        foreach (Tile tile in board.Tiles)
        {
            if(tile.IsRevealed) revealdTiles++;
        }
        revealdTiles.Should().Be(1);
    }
}
