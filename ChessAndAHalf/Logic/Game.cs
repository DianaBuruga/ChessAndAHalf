using ChessAndAHalf.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Logic
{
    public class Game
    {
        public Board Board { get; private set; }

        public Game()
        {
            Board = new Board();
        }
    }
}
