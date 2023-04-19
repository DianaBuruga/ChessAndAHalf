using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model.Pieces
{
    internal class SpeedyKnight : Piece
    {
        public SpeedyKnight(PlayerColor color) : base(color)
        {

        }
        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteSpeedyKnight.png" : "../../Images/Pieces/blackSpeedyKnight.png";
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
