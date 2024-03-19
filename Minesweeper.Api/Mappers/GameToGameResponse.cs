using Minesweeper.Api.Models;

namespace Minesweeper.Api.Mappers;

public class GameToGameResponse
{
    public GameResponse Map(Game game)
    {
        var gameResponse = new GameResponse()
        {
            GameId = game.Id,
            Board = new Board()
        };

        for (int row = 0; row < game.Board.Rows; row++)
        {
            for (int col = 0; col < game.Board.Columns; col++)
            {
                gameResponse.Board.Tiles.Add(
                    new Tile()
                    {
                        Col = col,
                        Row = row,
                        IsRevealed = game.Board.Tiles[row, col].IsRevealed,
                        IsFlagged = game.Board.Tiles[row, col].IsFlagged,
                        AdjacentMines = game.Board.Tiles[row, col].AdjacentMines
                    }
                );
            }
        }

        return gameResponse;
    }
}
