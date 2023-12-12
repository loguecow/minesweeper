using Minesweeper.Models;

namespace Minesweeper
{
    public class MimesweeperGame
    {
        public DateTime? StartTime { get; private set; } = null;
        private Board _board { get; set; }

        public MimesweeperGame(IBoardGenerator boardGenerator)
        {
            _board = boardGenerator.GenerateBoard();
        }

        public void RevealTile(int column, int row)
        {
            if(StartTime == null)
                StartTime = DateTime.Now;

            _board.RevealTile(column, row);
        }
    }
}
