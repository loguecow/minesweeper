import React from 'react';
import Tile from '../Tile';
import { Tile as TileModel } from '../../api/models';

interface BoardProps {
  rows: number;
  columns: number;
  tileData: TileModel[];
  onTileLeftClick: (row: number, column: number) => void;
  onTileRightClick: (row: number, column: number) => void;
}

const Board: React.FC<BoardProps> = ({ rows, columns, tileData, onTileLeftClick, onTileRightClick }) => {
  const renderBoard = () => {
    let board = [];
    for (let i = 0; i < rows; i++) {
      let row = [];
      for (let j = 0; j < columns; j++) {
        const tile = tileData.find(tile => tile.row === i && tile.col === j);
        row.push(
          <td key={`${i}-${j}`}>
            {tile && (
              <Tile
                tile={tile}
                onReveal={onTileLeftClick}
                onFlag={onTileRightClick}
              />
            )}
          </td>
        );
      }
      board.push(<tr key={i}>{row}</tr>);
    }
    return board;
  };

  return (
    <table>
      <tbody>
        {renderBoard()}
      </tbody>
    </table>
  );
};

export default Board;