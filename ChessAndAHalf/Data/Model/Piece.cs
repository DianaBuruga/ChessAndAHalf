using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model
{
    public abstract class Piece
    {
        public bool IsFirstMove { get; set; }
        public PlayerColor Color { get; set; }
        public abstract string GetImagePath();
        public abstract List<Position> GetLegalMoves(Board board, Square currentSquare);

        public Piece(PlayerColor color)
        {
            Color = color;
        }
    }
}
