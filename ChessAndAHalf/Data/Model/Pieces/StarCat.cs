using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model.Pieces
{
    public class StarCat : Piece
    {
        public StarCat(PlayerColor color) : base(color)
        {
        }

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteStarcat.png" : "../../Images/Pieces/blackStarcat.png";
        }
    }
}
