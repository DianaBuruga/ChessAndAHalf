using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model.Pieces
{
    class King : Piece
    {
        public King(PlayerColor color) : base(color)
        {
        }

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteKing.png" : "../../Images/Pieces/blackKing.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            List<Position> legalMoves = new List<Position>();
            //discutat mutari legale
            return legalMoves;
        }
    }
}
