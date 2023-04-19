using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model
{
    public abstract class Piece
    {
        public PlayerColor Color { get; set; }
        public abstract string GetImagePath();
        public abstract List<Position> GetLegalMoves(Board board, Square currentSquare);

        public Piece(PlayerColor color)
        {
            Color = color;
        }
    }
}
