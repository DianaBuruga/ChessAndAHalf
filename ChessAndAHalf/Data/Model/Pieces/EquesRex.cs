using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model.Pieces
{
    class EquesRex : Piece
    {
        public EquesRex(PlayerColor color) : base(color)
        {
        }

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteEquesRex.png" : "../../Images/Pieces/blackEquesRex.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            List<Position> legalMoves = new List<Position>();
            //discutat mutari legale
            return legalMoves;
        }
    }
}
