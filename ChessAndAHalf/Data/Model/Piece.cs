using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model
{
    public abstract class Piece
    {
        public PlayerColor Color { get; set; }
        public abstract string GetImagePath();

        public Piece(PlayerColor color)
        {
            Color = color;
        }
    }
}
