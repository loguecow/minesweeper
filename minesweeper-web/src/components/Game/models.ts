export interface ApiResponse {
  gameId: string
  board: Board
  mineExploded: boolean
  gameWon: boolean
  mines: number
}

export interface Board {
  tiles: Tile[]
}

export interface Tile {
  row: number
  col: number
  isRevealed: boolean
  isFlagged: boolean
  adjacentMines: number
  exploded: boolean
}
