import React from 'react';
import { ApiResponse, Tile } from './models';
import { FaFlag } from "react-icons/fa";
import exploded from '../../img/exploded.gif'

interface BoardProps {
  tileData: Tile[];
  apiResponse: ApiResponse | null;
  getIconForAdjacentMines: (adjacentMines: number) => JSX.Element | null;
  revealTile: (row: number, col: number) => void;
  flagTile: (row: number, col: number) => void;
  unflagTile: (row: number, col: number) => void;
}

const Board: React.FC<BoardProps> = ({ tileData, apiResponse, getIconForAdjacentMines, revealTile, flagTile, unflagTile }) => {
  const handleLeftClick = (row: number, col: number) => {
    if (!apiResponse || !apiResponse.mineExploded) {
      revealTile(row, col);
    }
  };

  const handleRightClick = (event: React.MouseEvent, row: number, col: number) => {
    event.preventDefault();
    if (!apiResponse || !apiResponse.mineExploded) {
      const tile = tileData.find(t => t.row === row && t.col === col);
      if (tile) {
        tile.isFlagged ? unflagTile(row, col) : flagTile(row, col);
      }
    }
  };

  const columns = Math.sqrt(tileData.length);

  return (
    <div className="board" style={{ gridTemplateColumns: `repeat(${columns}, 0fr)` }}>
    {tileData.map((tile, index) => (
      <div key={index} 
      className="tile"
      style={tile.isRevealed || tile.exploded ? { backgroundColor: '#a9a9a9' } : {backgroundColor: '#d3d3d3'}}
      onContextMenu={(event) => event.preventDefault()}
      >
        {tile.isRevealed ? (
          tile.exploded ? (
            <img src={exploded} alt="mineexplosion" />
          ) : (
            getIconForAdjacentMines(tile.adjacentMines)
          )
        ) : (
          <button 
            onClick={() => handleLeftClick(tile.row, tile.col)}
            onContextMenu={(event) => handleRightClick(event, tile.row, tile.col)}
          >
            {tile.isFlagged ? <FaFlag /> : ' '}
            {tile.exploded ? <img src={exploded} alt="mineexplosion" style={{width: '100%'}}/> : ' '}
          </button>
        )}
      </div>
    ))}
  </div>
  );
};

export default Board;