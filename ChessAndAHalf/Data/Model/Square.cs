using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model
{
    public class Square
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Piece Occupant { get; set; }

        public Square(int row, int column, Piece occupant = null)
        {
            Row = row;
            Column = column;
            Occupant = occupant;
        }
    }
}
