using FluentAssertions;
using Minesweeper.Implementation;

namespace Minesweeper.Tests;
public class GameTests
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
    public void RevealTile_FirstTile_ShouldStartTimer()
    {
        IBoardGenerator boardGenerator = new StaticBoardGenerator(BOARD_DEFINITION);
        Game game = new(boardGenerator, new Player());

        game.RevealTile(3, 3);

        game.StartTime.Should().NotBeNull();
    }

    [Fact]
    public void RevealTile_Mine_ShouldEndGame()
    {
        IBoardGenerator boardGenerator = new StaticBoardGenerator(BOARD_DEFINITION);
        Game game = new(boardGenerator, new Player());
        
        var action = () => game.RevealTile(1, 2);
        
        action.Should().Throw<MineExplodedException>();
        game.EndTime.Should().NotBeNull();
    }

    [Fact]
    public async Task FlagTile_AllTilesCorrectlyFlagged_GameWon()
    {
        IBoardGenerator boardGenerator = new StaticBoardGenerator(BOARD_DEFINITION);
        Game game = new(boardGenerator, new Player());

        game.FlagTile(0, 6);
        await Task.Delay(1000);
        game.FlagTile(1, 2);
        game.FlagTile(1, 3);
        game.FlagTile(2, 6);
        game.FlagTile(3, 0);
        game.FlagTile(3, 4);
        game.FlagTile(4, 2);
        game.FlagTile(5, 1);
        game.FlagTile(5, 2);
        game.FlagTile(5, 3);
        
        game.GetSecondsUsed().Should().BeGreaterThan(0);
    }

    [Fact]
    public void NewGame_SamePlayer_ShouldHaveUniuqeId()
    {
        IBoardGenerator boardGenerator = new StaticBoardGenerator(BOARD_DEFINITION);
        
        var player = new Player() { Name = "Calvin" };
        Game game1 = new(boardGenerator, player);
        Game game2 = new(boardGenerator, player);
     
        game1.Id.Should().NotBe(game2.Id);
    }
}
