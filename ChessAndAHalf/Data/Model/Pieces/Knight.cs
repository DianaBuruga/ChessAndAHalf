using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model.Pieces
{
    internal class Knight : Piece
    {
        public Knight(PlayerColor color) : base(color)
        {

        }
        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteKnight.png" : "../../Images/Pieces/blackKnight.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            List<Position> legalMoves = new List<Position>();
            /*
            mutari legale
            */
            return legalMoves;
        }
    }
}
