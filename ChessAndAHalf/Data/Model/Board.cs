using ChessAndAHalf.Data.Model.Pieces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ChessAndAHalf.Data.Model
{
    public class Board
    {
        private Square[,] squares;
        public int Size { get; }

        public Board()
        {
            Size = 12;

            squares = new Square[Size, Size];

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    squares[row, col] = new Square(row, col);
                }
            }

            squares[0, 0].Occupant = new Cat(PlayerColor.WHITE);
            squares[0, 1].Occupant = new StarCat(PlayerColor.WHITE);
            squares[0, 2].Occupant = new Cat(PlayerColor.BLACK);
            squares[0, 3].Occupant = new StarCat(PlayerColor.BLACK);
        }

        public Square GetSquare(int row, int col)
        {
            return squares[row, col];
        }
    }
}
