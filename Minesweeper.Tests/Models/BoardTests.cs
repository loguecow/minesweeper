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

    [Fact]
    public void RevealTile_RevealAll_GameWon()
    {
        // Arrange
        Board board = new StaticBoardGenerator(BOARD_DEFINITION).GenerateBoard();

        RevealAllTiles(board);

        board.GameWon.Should().BeTrue();
    }

    private static void RevealAllTiles(Board board)
    {
        // Act move 1
        board.RevealTile(0, 0);

        // Assert move 1
        board.Tiles[0, 0].IsRevealed.Should().BeTrue();
        board.Tiles[0, 1].IsRevealed.Should().BeTrue();
        board.Tiles[0, 1].AdjacentMines.Should().Be(1);
        board.Tiles[1, 0].IsRevealed.Should().BeTrue();
        board.Tiles[1, 1].IsRevealed.Should().BeTrue();
        board.Tiles[1, 1].AdjacentMines.Should().Be(1);
        board.Tiles[2, 0].IsRevealed.Should().BeTrue();
        board.Tiles[2, 0].AdjacentMines.Should().Be(1);
        board.Tiles[2, 1].IsRevealed.Should().BeTrue();
        board.Tiles[2, 1].AdjacentMines.Should().Be(2);

        // Act move 2
        board.RevealTile(0, 2);

        // Assert move 2
        board.Tiles[0, 2].IsRevealed.Should().BeTrue();
        board.Tiles[0, 2].AdjacentMines.Should().Be(2);

        // Act move 3
        board.RevealTile(0, 3);

        // Assert move 3
        board.Tiles[0, 3].IsRevealed.Should().BeTrue();
        board.Tiles[0, 3].AdjacentMines.Should().Be(2);

        // Act move 4
        board.RevealTile(0, 4);

        // Assert move 4
        board.Tiles[0, 4].IsRevealed.Should().BeTrue();
        board.Tiles[0, 4].AdjacentMines.Should().Be(1);

        // Act move 5
        board.RevealTile(0, 5);

        // Assert move 5
        board.Tiles[0, 5].IsRevealed.Should().BeTrue();
        board.Tiles[0, 5].AdjacentMines.Should().Be(1);

        // Act move 6
        board.RevealTile(2, 2);

        // Assert move 6
        board.Tiles[2, 2].IsRevealed.Should().BeTrue();
        board.Tiles[2, 2].AdjacentMines.Should().Be(2);

        // Act move 7
        board.RevealTile(2, 3);

        // Assert move 7
        board.Tiles[2, 3].IsRevealed.Should().BeTrue();
        board.Tiles[2, 3].AdjacentMines.Should().Be(3);

        // Act move 8
        board.RevealTile(2, 4);

        // Assert move 8
        board.Tiles[2, 4].IsRevealed.Should().BeTrue();
        board.Tiles[2, 4].AdjacentMines.Should().Be(2);

        // Act move 9
        board.RevealTile(2, 5);

        // Assert move 9
        board.Tiles[2, 5].IsRevealed.Should().BeTrue();
        board.Tiles[2, 5].AdjacentMines.Should().Be(2);

        // Act move 10
        board.RevealTile(1, 4);

        // Assert move 10
        board.Tiles[1, 4].IsRevealed.Should().BeTrue();
        board.Tiles[1, 4].AdjacentMines.Should().Be(1);

        // Act move 11
        board.RevealTile(1, 5);

        // Assert move 11
        board.Tiles[1, 5].IsRevealed.Should().BeTrue();
        board.Tiles[1, 5].AdjacentMines.Should().Be(2);

        // Act move 12
        board.RevealTile(1, 6);

        // Assert move 12
        board.Tiles[1, 6].IsRevealed.Should().BeTrue();
        board.Tiles[1, 6].AdjacentMines.Should().Be(2);

        // Act move 13
        board.RevealTile(3, 1);

        // Assert move 13
        board.Tiles[3, 1].IsRevealed.Should().BeTrue();
        board.Tiles[3, 1].AdjacentMines.Should().Be(2);

        // Act move 14
        board.RevealTile(3, 2);

        // Assert move 14
        board.Tiles[3, 2].IsRevealed.Should().BeTrue();
        board.Tiles[3, 2].AdjacentMines.Should().Be(1);

        // Act move 15
        board.RevealTile(3, 3);

        // Assert move 15
        board.Tiles[3, 3].IsRevealed.Should().BeTrue();
        board.Tiles[3, 3].AdjacentMines.Should().Be(2);

        // Act move 16
        board.RevealTile(3, 5);

        // Assert move 16
        board.Tiles[3, 5].IsRevealed.Should().BeTrue();
        board.Tiles[3, 5].AdjacentMines.Should().Be(2);

        // Act move 17
        board.RevealTile(3, 6);

        // Assert move 17
        board.Tiles[3, 6].IsRevealed.Should().BeTrue();
        board.Tiles[3, 6].AdjacentMines.Should().Be(1);

        // Act move 18
        board.RevealTile(5, 5);

        // Assert move 18
        board.Tiles[5, 5].IsRevealed.Should().BeTrue();
        board.Tiles[5, 5].AdjacentMines.Should().Be(0);
        board.Tiles[4, 4].IsRevealed.Should().BeTrue();
        board.Tiles[4, 4].AdjacentMines.Should().Be(2);
        board.Tiles[4, 5].IsRevealed.Should().BeTrue();
        board.Tiles[4, 5].AdjacentMines.Should().Be(1);
        board.Tiles[4, 6].IsRevealed.Should().BeTrue();
        board.Tiles[4, 6].AdjacentMines.Should().Be(0);
        board.Tiles[5, 4].IsRevealed.Should().BeTrue();
        board.Tiles[5, 4].AdjacentMines.Should().Be(1);
        board.Tiles[5, 6].IsRevealed.Should().BeTrue();
        board.Tiles[5, 6].AdjacentMines.Should().Be(0);
        board.Tiles[6, 4].IsRevealed.Should().BeTrue();
        board.Tiles[6, 4].AdjacentMines.Should().Be(1);
        board.Tiles[6, 5].IsRevealed.Should().BeTrue();
        board.Tiles[6, 5].AdjacentMines.Should().Be(0);
        board.Tiles[6, 6].IsRevealed.Should().BeTrue();
        board.Tiles[6, 6].AdjacentMines.Should().Be(0);

        // Act move 19
        board.RevealTile(4, 3);

        // Assert move 19
        board.Tiles[4, 3].IsRevealed.Should().BeTrue();
        board.Tiles[4, 3].AdjacentMines.Should().Be(4);

        // Act move 20
        board.RevealTile(6, 3);

        // Assert move 20
        board.Tiles[6, 3].IsRevealed.Should().BeTrue();
        board.Tiles[6, 3].AdjacentMines.Should().Be(2);

        // Act move 21
        board.RevealTile(6, 2);

        // Assert move 21
        board.Tiles[6, 2].IsRevealed.Should().BeTrue();
        board.Tiles[6, 2].AdjacentMines.Should().Be(3);

        // Act move 22
        board.RevealTile(6, 1);

        // Assert move 22
        board.Tiles[6, 1].IsRevealed.Should().BeTrue();
        board.Tiles[6, 1].AdjacentMines.Should().Be(2);

        // Act move 23
        board.RevealTile(6, 0);

        // Assert move 23
        board.Tiles[6, 0].IsRevealed.Should().BeTrue();
        board.Tiles[6, 0].AdjacentMines.Should().Be(1);

        // Act move 24
        board.RevealTile(5, 0);

        // Assert move 24
        board.Tiles[5, 0].IsRevealed.Should().BeTrue();
        board.Tiles[5, 0].AdjacentMines.Should().Be(1);

        // Act move 25
        board.RevealTile(4, 0);

        // Assert move 25
        board.Tiles[4, 0].IsRevealed.Should().BeTrue();
        board.Tiles[4, 0].AdjacentMines.Should().Be(2);

        // Act move 26
        board.RevealTile(4, 1);

        // Assert move 26
        board.Tiles[4, 1].IsRevealed.Should().BeTrue();
        board.Tiles[4, 1].AdjacentMines.Should().Be(4);
    }
}
