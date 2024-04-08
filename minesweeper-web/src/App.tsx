import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

import { ApiResponse, Tile } from './models';

import { RiCheckboxBlankFill } from "react-icons/ri";
import { FaFlag } from "react-icons/fa";
import { Fa1, Fa2, Fa3, Fa4, Fa5, Fa6, Fa7, Fa8 } from "react-icons/fa6";

function App() {
  const [apiResponse, setApiResponse] = useState<ApiResponse | null>(null);
  const [tileData, setTileData] = useState<Tile[]>([]);
  const [level, setLevel] = useState(0);
  const [gameId, setGameId] = useState<string | null>(null);

  const API_HOST = 'http://localhost:5008';

  const CreateNewGame = () => {
    setApiResponse(null);
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
  
  const MakeMove = () => {

    const MoveData = 
    {
      row: 0,
      column: 2,
      move: 0
    };
    axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, MoveData)
    .then(response => response.data)
    .then(data => {
      setApiResponse(data);
      if (data.board && data.board.tiles) {
        setTileData(data.board.tiles);
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
  return (
    <div className="App">
      <header className="App-header">
        <p>
          Minesweeep
        </p>
        <select value={level} onChange={(e) => setLevel(Number(e.target.value))}>
        <option value={0}>Beginner</option>
        <option value={1}>Intermediate</option>
        <option value={2}>Expert</option>
      </select>
        <button onClick={CreateNewGame}>Create game</button>
        <button onClick={MakeMove}>Make move test</button>

        <pre>{apiResponse?.gameId}</pre>
        <pre>{apiResponse?.mineExploded}</pre>
        <table>
          <tbody>
            <div className='grid'>

            {tileData.map((tile, index) => (
            <tr key={index}>
              <a 
              onDrag={(event) => event.preventDefault()}
              onClick={() => revealTile(tile.row, tile.col)}
              onContextMenu={(event) => {
                event.preventDefault();
                flagTile(tile.row, tile.col);
              }}
              style={{border: '1px solid black', width: '16px', height: '16px',backgroundColor: tile.exploded ? 'red' : (tile.isRevealed ? 'green' : 'white')}}>
              {tile.isFlagged ? <FaFlag /> : (tile.isRevealed ? getIconForAdjacentMines(tile.adjacentMines) : <RiCheckboxBlankFill />)}
              </a>
            </tr>
          ))}
            </div>
          </tbody>
        </table>
      </header>
    </div>
  );
}

export default App;