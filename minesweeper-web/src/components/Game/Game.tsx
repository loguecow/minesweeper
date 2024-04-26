import React, { useEffect, useRef, useState } from 'react';
import { createNewGame, flagTile, revealTile, unflagTile } from './Api';
import Board from './Board';
import { ApiResponse, Tile } from './models';
import '../../css/Game.css';

import { ToggleGroup } from '@digdir/designsystemet-react';

import faceClicked from '../../img/faceclicked.gif';
import deadlogo from '../../img/deadlogo.gif';
import faceWon from '../../img/facewon.gif';

function Game() {
  const [apiResponse, setApiResponse] = useState<ApiResponse | null>(null);
  const [tileData, setTileData] = useState<Tile[]>([]);
  const [mines, setMines] = useState(0);
  const [level, setLevel] = useState(0);
  const [face, setFace] = useState('normal');
  const [seconds, setSeconds] = useState(0);
  const [gameId, setGameId] = useState<string | null>('');
  const intervalRef = useRef<NodeJS.Timeout | null>(null);

  useEffect(() => {
    createNewGame(level).then(data => {
      setApiResponse(data);
      setTileData(data.board.tiles);
      setGameId(data.gameId);
      setFace('normal');
      setSeconds(0);
    });
  }, [level]);

  useEffect(() => {
    if (apiResponse && apiResponse.board && apiResponse.board.tiles) {
      setMines(apiResponse.mines);
      setTileData(apiResponse.board.tiles);
      if(apiResponse.board.tiles.find(tile => tile.isRevealed)) {
        startTimer();
      }
    }
    if (apiResponse?.mineExploded) {
      setFace('exploded');
      stopTimer();
    }
    if (apiResponse?.gameWon) {
      setFace('won');
      stopTimer();
    }
  }, [apiResponse]);

  const startTimer = () => {
    if (intervalRef.current !== null) return;
    intervalRef.current = setInterval(() => {
      setSeconds(seconds => seconds < 999 ? seconds + 1 : 999);
    }, 1000);
  };

  const stopTimer = () => {
    if (intervalRef.current !== null) {
      clearInterval(intervalRef.current);
      intervalRef.current = null;
    }
  };

  function getIconForAdjacentMines(adjacentMines: number) {
    if (!adjacentMines) return null;
    const colors = ['blue', 'green', 'red', 'darkblue', 'brown', 'cyan', 'black', 'grey'];
    return <span style={{ color: colors[adjacentMines - 1] }}>{adjacentMines}</span>;
  }

  const flaggedTiles = tileData.filter(tile => tile.isFlagged).length;

  const flaggedminesleft = mines - flaggedTiles;

  let faceImage;
  switch (face) {
    case 'normal':
      faceImage = 'https://freeminesweeper.org/images/facesmile.gif';
      break;
    case 'clicked':
      faceImage = faceClicked;
      break;
    case 'exploded':
      faceImage = deadlogo;
      break;
    case 'won':
      faceImage = faceWon;
      break;
  }

  return (
    <div className="game">
    <h1>Minesweeper</h1>
    <div className="info">
      <div className='timer'>{String(seconds).padStart(3, '0')}</div>
      <button onClick={() => window.location.reload()}><img src={faceImage}/></button>
      <div className='flaggedminesleft'>{String(flaggedminesleft).padStart(3, '0')}</div>
    </div>
      <ToggleGroup defaultValue='0' onChange={(value) => setLevel(Number(value))}>
        <ToggleGroup.Item value='0'>Beginner</ToggleGroup.Item>
        <ToggleGroup.Item value='1'>Intermediate</ToggleGroup.Item>
        <ToggleGroup.Item value='2'>Expert</ToggleGroup.Item>
      </ToggleGroup>
    <Board 
      tileData={tileData} 
      apiResponse={apiResponse}
      getIconForAdjacentMines={getIconForAdjacentMines}
      revealTile={(row, col) => revealTile(gameId ?? '',row, col).then(data => setApiResponse(data))}
      flagTile={(row, col) => flagTile(gameId ?? '', row, col).then(data => setApiResponse(data))}
      unflagTile={(row, col) => unflagTile(gameId ?? '',row, col).then(data => setApiResponse(data))}
    />
  </div>
  );
}

export default Game;