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

            squares[4, 4].Occupant = new Cat(PlayerColor.WHITE);
            squares[0, 1].Occupant = new StarCat(PlayerColor.WHITE);
            squares[8, 8].Occupant = new Cat(PlayerColor.BLACK);
            squares[0, 3].Occupant = new StarCat(PlayerColor.BLACK);
        }

        public Square GetSquare(int row, int col)
        {
            if (row < 0 || col < 0 || row > Size - 1 || col > Size - 1)
                return null;

            return squares[row, col];
        }

        public void ClearHighlight()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    squares[row, col].IsHighlighted = false;
                }
            }
        }
    }
}
