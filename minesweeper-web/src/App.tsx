import React, { useEffect, useRef, useState } from 'react';
import axios from 'axios';
import './css/App.css';
import deadlogo from './img/faceexploded.gif'
import faceClicked from './img/faceclicked.gif'
import faceWon from './img/facewon.gif'
import exploded from './img/exploded.gif'

import { ApiResponse, Tile } from './models';

import { RiCheckboxBlankFill } from "react-icons/ri";
import { FaFlag } from "react-icons/fa";
import { Fa1, Fa2, Fa3, Fa4, Fa5, Fa6, Fa7, Fa8 } from "react-icons/fa6";
import { GiMineExplosion } from "react-icons/gi";
import { VscBlank } from "react-icons/vsc";

function App() {
  const [apiResponse, setApiResponse] = useState<ApiResponse | null>(null);
  const [gameData, setGameData] = useState<ApiResponse[] | null>(null);
  const [tileData, setTileData] = useState<Tile[]>([]);
  const [level, setLevel] = useState(0);
  const [gameId, setGameId] = useState<string | null>(null);
  const [gameWon, setGameWon] = useState(false);
  const [rows, setRows] = useState<number>(9); // Default number of rows
  const [columns, setColumns] = useState<number>(9); // Default number of columns
  const [face, setFace] = useState('normal');
  const [seconds, setSeconds] = useState(0);
  const intervalRef = useRef<NodeJS.Timeout | null>(null);


  const API_HOST = 'http://localhost:5008';

  useEffect(() => {
    CreateNewGame();
  }, [level]);
  
  
  useEffect(() => {
    checkVictory();
  }, [apiResponse]);
  


  const CreateNewGame = () => {
    stopTimer();
    setSeconds(0);
    setApiResponse(null);
    setFace('normal')
    setGameWon(false)
    setTileData([]);
    const data = 
    {
      level: level,
      userName: "Andreas",
    };
  
    axios.post<ApiResponse>(`${API_HOST}/games/`, data)
    .then(response => response.data)
    .then(data => {
      setApiResponse(data); // Update and display the new data
      setGameId(data.gameId)
      if (data.board && data.board.tiles) {
        setTileData(data.board.tiles); // Set tileData to the new tiles
        const maxRow = Math.max(...data.board.tiles.map(tile => tile.row));
        const maxCol = Math.max(...data.board.tiles.map(tile => tile.col));
        setRows(maxRow + 1);
        setColumns(maxCol + 1);
      }
    })
    .catch(error => setApiResponse(error.toString()));
  };
  
  const revealTile = (row: number, column: number) => {
    if (!intervalRef.current) {
      startTimer();
    }
    const MoveData = 
    {
      row: row,
      column: column,
      move: 0
    };
    axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, MoveData)
    .then(response => response.data)
    .then(data => {
      setApiResponse(data); // Update and display the new data
      if (data.board && data.board.tiles) {
        setTileData([]); // Clear the old tile data
        for (let tile of data.board.tiles) {
          setTileData(prevData => [...prevData, tile]);
        }
        if (data.mineExploded) {
          setFace('exploded');
          stopTimer();
        } else if (gameWon) {
          setFace('won');
        } else {
          setFace('normal');
        }
      }
    })
    .catch(error => setApiResponse(error.toString()));
  };

  const flagTile = (row: number, column: number) => {

    const tile = tileData.find(tile => tile.row === row && tile.col === column);
    if (tile && tile.isRevealed) {
      return;
    }
    const MoveData = 
    {
      row: row,
      column: column,
      move: 1
    };
    axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, MoveData)
    .then(response => response.data)
    .then(data => {
      setApiResponse(data); // Update and display the new data
      if (data.board && data.board.tiles) {
        setTileData([]); // Clear the old tile data
        for (let tile of data.board.tiles) {
          setTileData(prevData => [...prevData, tile]);
        }
      }
    })
    .catch(error => setApiResponse(error.toString()));
  };

  const unflagTile = (row: number, column: number) => {
    const MoveData = 
    {
      row: row,
      column: column,
      move: 2
    };
    axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, MoveData)
    .then(response => response.data)
    .then(data => {
      setApiResponse(data); // Update and display the new data
      if (data.board && data.board.tiles) {
        setTileData([]); // Clear the old tile data
        for (let tile of data.board.tiles) {
          setTileData(prevData => [...prevData, tile]);
        }
      }
    })
    .catch(error => setApiResponse(error.toString()));
  };

  function getIconForAdjacentMines(adjacentMines: number) {
    switch (adjacentMines) {
      case 1:
        return <Fa1  color='blue'/>;
      case 2:
        return <Fa2 color='green'/>;
      case 3:
        return <Fa3 color='red'/>;
      case 4:
        return <Fa4 color='darkblue'/>;
      case 5:
        return <Fa5 color='brown'/>;
      case 6:
        return <Fa6 color='cyan'/>;
      case 7:
        return <Fa7 color='black'/>;
      case 8:
        return <Fa8 color='grey'/>;
      default:
        return null;
    }
  }

  let _mines: number ;
  const flaggedTiles = tileData.filter(tile => tile.isFlagged).length;
  switch(level) {
    case 0:
      _mines = 10;
      break;
      case 1:
        _mines = 30;
        break;
        case 2:
          _mines = 90;
          break;
          default:
            _mines = 10;
          }
          
  const minesLeft = _mines - flaggedTiles;

  const checkVictory = () => {
    if (apiResponse?.gameWon) {
      setGameWon(true);
      stopTimer();
      setFace('won');
    }
  };

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

  const startTimer = () => {
    if (intervalRef.current !== null) return; // Timer already started
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
  
  return (
    <div className="Game">
        <div className='Board'>
        <table>
              <thead>
            <tr>
              <th colSpan={columns}>
                <header className="game-menu">
                  <p>Minesweeep</p>
                  <select value={level} onChange={(e) => {
                    setLevel(Number(e.target.value))
                    CreateNewGame()
                  }}>
                    <option value={0}>Beginner</option>
                    <option value={1}>Intermediate</option>
                    <option value={2}>Expert</option>
                  </select>
                  <div style={{display: 'flex'}}>
                  {apiResponse?.board != null && <div className='timer'>{String(seconds).padStart(3, '0')}</div>}
                  <div className='faceImage'><a onClick={CreateNewGame}><img src={faceImage}/></a></div>
                  {apiResponse?.board != null && <div className='flagleft'>{minesLeft}</div>}
                  </div>
                  {gameWon && <div>Game Won!</div>}
                </header>
              </th>
            </tr>
          </thead>
          <tbody>
          {Array.from({ length: rows }, (_, rowIndex) => (
            <tr key={rowIndex}>
              {Array.from({ length: columns }, (_, colIndex) => {
                const tile = tileData.find(t => t.row === rowIndex && t.col === colIndex);
                return tile ? (
                  <td 
                    key={colIndex}
                    onDrag={(event) => event.preventDefault()}
                    onMouseDown={(event) => {
                      event.preventDefault();
                      if (apiResponse?.mineExploded || apiResponse?.gameWon) {
                        console.log('Game Over');
                        return;
                      }
                      if (event.button === 0) {
                        if (!tile.isFlagged) {
                          revealTile(tile.row, tile.col);
                          setFace('clicked')
                        }
                      } else if (event.button === 2) {
                        if (tile.isFlagged) {
                          unflagTile(tile.row, tile.col);
                        } else {
                          flagTile(tile.row, tile.col);
                        }
                      }
                      checkVictory();
                    }}
                    onContextMenu={(event) => event.preventDefault()}
                    style={{
                      backgroundColor: tile.exploded ? 'red' : (tile.isFlagged ? '#ddd' : (tile.isRevealed ? '#ddd' : 'white'))
                    }}
                  >
                    <div id='tile'>
                      {tile.exploded ? <img src={exploded}/> : null}
                      {tile.isFlagged ? <FaFlag color="red"/> : (tile.isRevealed ? getIconForAdjacentMines(tile.adjacentMines) : null)}
                    </div>
                  </td>
                ) : null;
              })}
            </tr>
          ))}
          </tbody>
        </table>
        </div>
    </div>
  );
}

export default App;