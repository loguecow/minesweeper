import React, { useState } from 'react';
import axios from 'axios';
import './App.css';
import deadlogo from './nahh-nah.gif'

import { ApiResponse, Tile } from './models';

import { RiCheckboxBlankFill } from "react-icons/ri";
import { FaFlag } from "react-icons/fa";
import { Fa1, Fa2, Fa3, Fa4, Fa5, Fa6, Fa7, Fa8 } from "react-icons/fa6";
import { GiMineExplosion } from "react-icons/gi";

function App() {
  const [apiResponse, setApiResponse] = useState<ApiResponse | null>(null);
  const [tileData, setTileData] = useState<Tile[]>([]);
  const [level, setLevel] = useState(0);
  const [gameId, setGameId] = useState<string | null>(null);
  const [gameWon, setGameWon] = useState(false);


  const API_HOST = 'http://localhost:5008';

  const CreateNewGame = () => {
    setApiResponse(null);
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
      }
    })
    .catch(error => setApiResponse(error.toString()));
  };
  
  const revealTile = (row: number, column: number) => {
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
        return <Fa1 />;
      case 2:
        return <Fa2/>;
      case 3:
        return <Fa3/>;
      case 4:
        return <Fa4/>;
      case 5:
        return <Fa5/>;
      case 6:
        return <Fa6/>;
      case 7:
        return <Fa7/>;
      case 8:
        return <Fa8/>;
      default:
        return <RiCheckboxBlankFill />;
    }
  }

  let _rows: number, _columns: number, _mines: number ;
  const flaggedTiles = tileData.filter(tile => tile.isFlagged).length;
  switch(level) {
    case 0:
      _rows = 9;
      _columns = 9;
      _mines = 10;
      break;
      case 1:
        _rows = 18;
        _columns = 18;
        _mines = 15;
        break;
        case 2:
          _rows = 60;
          _columns = 60;
          _mines = 90;
          break;
          default:
            _rows = 9;
            _columns = 9;
            _mines = 10;
          }
          
  const minesLeft = _mines - flaggedTiles;

  const checkVictory = () => {
    const totalTiles = _rows * _columns;
    const totalMines = _mines;
    const revealedTiles = tileData.filter(tile => tile.isRevealed).length;
  
    if (revealedTiles === totalTiles - totalMines) {
      setGameWon(true)
    }
  };

  return (
    <div className="App">
      <header className="App-header">
        <div className='game'>
        <p>
          Minesweeep
        </p>
        <select value={level} onChange={(e) => setLevel(Number(e.target.value))}>
        <option value={0}>Beginner</option>
        <option value={1}>Intermediate</option>
        <option value={2}>Expert</option>
      </select>
        <button onClick={CreateNewGame}>Create game</button>

        <pre>{apiResponse?.gameId}</pre>
        {apiResponse?.board != null && <div>Mines left: {minesLeft}</div>}
        {gameWon && <div>Game Won!</div>}
        {apiResponse?.mineExploded && <div><GiMineExplosion/><br/>Mine Exploded<br/><img src={deadlogo}/></div>}
        <table>
          <tbody>
          <div className='grid' style={{
          display: 'grid',
          gridTemplateColumns: `repeat(${_columns}, 1fr)`,
          gridTemplateRows: `repeat(${_rows}, 1fr)`,
          gap: '1px',
          overflow: 'hidden'
        }}>

            {tileData.map((tile, index) => (
            <tr key={index}>
              <a 
              onDrag={(event) => event.preventDefault()}
              onMouseDown={(event) => {
                event.preventDefault();
                if (apiResponse?.mineExploded) {
                  console.log('Game Over');
                  return;
                }
                if (event.button === 0) {
                  if (!tile.isFlagged) {
                    revealTile(tile.row, tile.col);
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
                border: '1px solid black',
                borderRadius: '3px',
                width: '16px', 
                height: '16px',
                backgroundColor: tile.exploded ? 'red' : (tile.isFlagged ? 'blue' : (tile.isRevealed ? 'green' : 'white'))}}>
              {tile.isFlagged ? <FaFlag /> : (tile.isRevealed ? getIconForAdjacentMines(tile.adjacentMines) : <RiCheckboxBlankFill />)}
              </a>
            </tr>
          ))}
            </div>
          </tbody>
        </table>
        </div>
      </header>
    </div>
  );
}

export default App;