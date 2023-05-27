using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model
{
    public class Move
    {
        public Position Tile { get; set; }
        public Position Next { get; set; }

        public Move() { }
        public Move(Position tile, Position next)
        {
            this.Tile = tile;
            this.Next = next;
        }
    }
}
