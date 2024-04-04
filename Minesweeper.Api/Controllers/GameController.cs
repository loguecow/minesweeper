using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Minesweeper.Api.Mappers;
using Minesweeper.Api.Models;

namespace Minesweeper.Api.Controllers;
[ApiController]
[Route("games")]
[EnableCors("AllowAnyOrigin")]
public class GameController : ControllerBase
{
    private readonly GameManager _gameManager;
    private readonly ILogger<Game> _logger;

    public GameController(GameManager gameManager, ILogger<Game> logger)
    {
        _gameManager = gameManager;
        _logger = logger;
    }

    [HttpGet]
    [Route("{gameId}", Name = "GetGameById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameResponse>> Get(Guid gameId)
    {
        Game game = _gameManager.LoadGame(gameId);

        if (game == null)
        {
            return NotFound();
        }

        GameResponse gameResponse = new GameToGameResponse().Map(game);

        return Ok(gameResponse);
    }

    [HttpPost(Name = "CreateNewGame")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GameResponse>> Post([FromBody]NewGameRequest newGameRequest)
    {
        if (newGameRequest == null)
        {
            return BadRequest();
        }

        _logger.LogInformation($"Starting new {newGameRequest.Level} level game for user {newGameRequest.UserId}");

        Game game = _gameManager.CreateNewGame(new Player(), newGameRequest.Level);
        GameResponse gameResponse = new GameToGameResponse().Map(game);

        return CreatedAtRoute("GetGameById", new { gameId = game.Id }, gameResponse);
    }

    [HttpPut]
    [Route("{gameId}", Name = "MakeMove")]
    public async Task<ActionResult<GameResponse>> Put(Guid gameId, MoveRequest moveRequest)
    {
        Game game = _gameManager.LoadGame(gameId);
        bool mineExploded = false;

        try
        {
            if (moveRequest.Move == Move.Flag)
            {
                game.FlagTile(moveRequest.Row, moveRequest.Column);
            }
            else if (moveRequest.Move == Move.Unflag)
            {
                game.UnflagTile(moveRequest.Row, moveRequest.Column);
            }
            else if (moveRequest.Move == Move.Reveal)
            {
                game.RevealTile(moveRequest.Row, moveRequest.Column);
            }
        }
        catch (MineExplodedException)
        {
            mineExploded = true;
        }
        
        GameResponse gameResponse = new GameToGameResponse().Map(game);
        gameResponse.MineExploded = mineExploded;
        _gameManager.SaveGame(game);
        
        return gameResponse;
    }
}
