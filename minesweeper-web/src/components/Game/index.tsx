import React, { useEffect } from 'react';
import { createNewGame, RevealTile, FlagTile } from '../../api/api';
import Level from './Level';
import Board from '../Board';
import { Tile as TileModel } from '../../api/models';

interface GameProps {
  userName: string;
  gameId: string;
}

const Game: React.FC<GameProps> = ({ userName, gameId }) => {
  const [level, setLevel] = React.useState(0);
  const [rows, setRows] = React.useState(9);
  const [columns, setColumns] = React.useState(9);
  const [mines, setMines] = React.useState(10);
  const [tileData, setTileData] = React.useState<TileModel[]>([]);

  const setGridSize = (rows: number, columns: number) => {
    setRows(rows);
    setColumns(columns);
  };

  const handleTileLeftClick = async (row: number, column: number) => {
    const response = await RevealTile(gameId, row, column);
    setTileData(response.data.board.tiles);
  };
  
  const handleTileRightClick = async (row: number, column: number) => {
    const response = await FlagTile(gameId, row, column);
    setTileData(response.data.board.tiles);
  };

  useEffect(() => {
    const startNewGame = async () => {
      const newGame = await createNewGame(level, userName);
      setTileData(newGame.data.board.tiles);
    };

    startNewGame();
  }, [level, userName, rows, columns, mines]);

  return (
    <div>
      <Level level={level} setLevel={setLevel} setGridSize={setGridSize} setMines={setMines} />
      <Board rows={rows} columns={columns} tileData={tileData} onTileLeftClick={handleTileLeftClick} onTileRightClick={handleTileRightClick} />
    </div>
  );
};

export default Game;