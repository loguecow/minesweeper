import React from 'react';
import { Tile as TileModel } from '../../api/models';

interface TileProps {
  tile: TileModel;
  onReveal: (row: number, col: number) => void;
  onFlag: (row: number, col: number) => void;
}

const Tile: React.FC<TileProps> = ({ tile, onReveal, onFlag }) => {
  const handleReveal = () => {
    onReveal(tile.row, tile.col);
  };

  const handleFlag = (e: React.MouseEvent) => {
    e.preventDefault();
    onFlag(tile.row, tile.col);
  };

  let content = '';
  if (tile.isRevealed) {
    if (tile.exploded) {
      content = '💥';
    } else if (tile.adjacentMines > 0) {
      content = tile.adjacentMines.toString();
    }
  } else if (tile.isFlagged) {
    content = '🚩';
  }

  return (
    <div onClick={handleReveal} onContextMenu={handleFlag}>
      {content}
    </div>
  );
};

export default Tile;