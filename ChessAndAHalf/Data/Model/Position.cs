using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Data.Model
{
    public class Position
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
