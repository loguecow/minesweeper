﻿using Microsoft.Extensions.Caching.Memory;
using Minesweeper.Implementation;
using Minesweeper.Models;

namespace Minesweeper;

public class GameManager
{
    private readonly IMemoryCache _memoryCache;

    public GameManager(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Game CreateNewGame(IPlayer player, Level level)
    {
        IBoardGenerator randomBoardGenerator = new RandomBoardGenerator(level);
        Game game = new(randomBoardGenerator, player);

        _memoryCache.Set(game.Id, game);

        return game;
    }

    public void SaveGame(Game game)
    {
        throw new NotImplementedException();
    }

    public Game LoadGame(Guid gameId)
    {
        throw new NotImplementedException();
    }
}
