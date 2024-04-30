import React, { useEffect, useRef, useState } from 'react';
import { createNewGame, flagTile, revealTile, unflagTile } from './Api';
import Board from './Board';
import { ApiResponse, Tile } from './models';
import '../../css/Game.css';

import '@digdir/designsystemet-theme';
import '@digdir/designsystemet-css';
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
  const intervalRef = useRef<NodeJS.Timeout | null>(null);
  const [isTimerActive, setTimerActive] = useState(false);

  
  const startGame = (level = 0) => {
    createNewGame(level).then(data => {
      setTimerActive(false);
      setApiResponse(data);
      setTileData(data.board.tiles);
      setFace('normal');
      setSeconds(0);
  });
  }

  useEffect(() => {
  startGame(level);
  }, [level]);

  useEffect(() => {
    if (apiResponse && apiResponse.board && apiResponse.board.tiles) {
      setMines(apiResponse.mines);
      setTileData(apiResponse.board.tiles);
      if(apiResponse.board.tiles.find(tile => tile.isRevealed)) {
        setTimerActive(true);
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

  useEffect(() => {
    const startTimer = () => {
      intervalRef.current = setInterval(() => {
        setSeconds(seconds => seconds < 999 ? seconds + 1 : 999);
      }, 1000);
    };
    
    if (isTimerActive){
      startTimer();
    }
  
    return () => {
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
      }
    };
  }, [isTimerActive]);

  const stopTimer = () => {
    if (intervalRef.current !== null) {
      setTimerActive(false);
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
    <div className="info">
      <div className='timer'>{String(seconds).padStart(3, '0')}</div>
      <button onClick={() => startGame(level)}><img src={faceImage}/></button>
      <div className='flaggedminesleft'>{String(flaggedminesleft).padStart(3, '0')}</div>
    </div>
      <ToggleGroup style={{minWidth:'190px', width: '190px'}} size="small" defaultValue='0' onChange={(value) => setLevel(Number(value))}>
        <ToggleGroup.Item value='0'>Easy</ToggleGroup.Item>
        <ToggleGroup.Item value='1'>Medium</ToggleGroup.Item>
        <ToggleGroup.Item value='2'>Hard</ToggleGroup.Item>
      </ToggleGroup>
    <Board 
      tileData={tileData} 
      apiResponse={apiResponse}
      getIconForAdjacentMines={getIconForAdjacentMines}
      revealTile={(row, col) => revealTile(apiResponse?.gameId ?? '',row, col).then(data => setApiResponse(data))}
      flagTile={(row, col) => flagTile(apiResponse?.gameId ?? '', row, col).then(data => setApiResponse(data))}
      unflagTile={(row, col) => unflagTile(apiResponse?.gameId ?? '',row, col).then(data => setApiResponse(data))}
    />
  </div>
  );
}

export default Game;