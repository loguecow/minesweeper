import axios from 'axios';
import { ApiResponse } from './models';

const API_HOST = 'http://localhost:5008';

export const createNewGame = (level: number, userName: string) => {
  const data = { level, userName };
  return axios.post<ApiResponse>(`${API_HOST}/games/`, data);
};

export const RevealTile = (gameId: string, row: number, column: number) => {
    const MoveData = { row, column, move: 0 };
    return axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, MoveData);
  };

export const FlagTile = (gameId: string, row: number, column: number) => {
  const MoveData = { row, column, move: 1 };
  return axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, MoveData);
};