import axios from 'axios';
import { ApiResponse } from './models';


const API_HOST = 'http://localhost:5008';

export const createNewGame = async (level: number) => {
  const data = {
    level: level,
    userName: "Andreas",
  };
  return axios.post<ApiResponse>(`${API_HOST}/games/`, data)
    .then(response => response.data);
};

export const revealTile = (gameId: string, row: number, column: number) => {
  const moveData = {
    row: row,
    column: column,
    move: 0
  };
  return axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, moveData)
    .then(response => response.data);
};

export const flagTile = (gameId: string, row: number, column: number) => {
  const moveData = {
    row: row,
    column: column,
    move: 1
  };
  return axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, moveData)
    .then(response => response.data);
};

export const unflagTile = (gameId: string, row: number, column: number) => {
  const moveData = {
    row: row,
    column: column,
    move: 2
  };
  return axios.put<ApiResponse>(`${API_HOST}/games/${gameId}`, moveData)
    .then(response => response.data);
};