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

            /* squares[4, 4].Occupant = new Cat(PlayerColor.WHITE);
             squares[0, 1].Occupant = new StarCat(PlayerColor.WHITE);
             squares[8, 8].Occupant = new Cat(PlayerColor.BLACK);
             squares[0, 3].Occupant = new StarCat(PlayerColor.BLACK);*/
            StartPlacement();
        }

        public void StartPlacement()
        {
            int column = 0, row =0;
            squares[row, column].Occupant = new Rook(PlayerColor.BLACK);
            squares[row, 11-column].Occupant = new Rook(PlayerColor.BLACK);
            squares[11,column].Occupant = new Rook(PlayerColor.WHITE);
            squares[11, 11-column].Occupant = new Rook(PlayerColor.WHITE);
            column++;
            squares[row, column].Occupant = new Knight(PlayerColor.BLACK);
            squares[row, 11 - column].Occupant = new Knight(PlayerColor.BLACK);
            squares[11, column].Occupant = new Knight(PlayerColor.WHITE);
            squares[11, 11 - column].Occupant = new Knight(PlayerColor.WHITE);
            column++;
            squares[row, column].Occupant = new Bishop(PlayerColor.BLACK);
            squares[row, 11 - column].Occupant = new Bishop(PlayerColor.BLACK);
            squares[11, column].Occupant = new Bishop(PlayerColor.WHITE);
            squares[11, 11 - column].Occupant = new Bishop(PlayerColor.WHITE);
            column++;
            squares[row, column].Occupant = new StarCat(PlayerColor.BLACK);
            squares[row, 11 - column].Occupant = new StarCat(PlayerColor.BLACK);
            squares[11, column].Occupant = new StarCat(PlayerColor.WHITE);
            squares[11, 11 - column].Occupant = new StarCat(PlayerColor.WHITE);
            column++;
            squares[row, column].Occupant = new Cat(PlayerColor.BLACK);
            squares[row, 11 - column].Occupant = new Cat(PlayerColor.BLACK);
            squares[11, column].Occupant = new Cat(PlayerColor.WHITE);
            squares[11, 11 - column].Occupant = new Cat(PlayerColor.WHITE);
            column++;
            squares[row, column].Occupant = new Queen(PlayerColor.BLACK);
            squares[row, 11 - column].Occupant = new King(PlayerColor.BLACK);
            squares[11, column].Occupant = new Queen(PlayerColor.WHITE);
            squares[11, 11 - column].Occupant = new King(PlayerColor.WHITE);
            row++;
            for(column = 0; column < 6; column++)
            {
                if (column == 3)
                {
                    squares[row, column].Occupant = new Guard(PlayerColor.BLACK);
                    squares[row, 11 - column].Occupant = new Guard(PlayerColor.BLACK);
                    squares[11-row, column].Occupant = new Guard(PlayerColor.WHITE);
                    squares[11-row, 11 - column].Occupant = new Guard(PlayerColor.WHITE);
                }
                else
                {
                    squares[row, column].Occupant = new Pawn(PlayerColor.BLACK);
                    squares[row, 11 - column].Occupant = new Pawn(PlayerColor.BLACK);
                    squares[11 - row, column].Occupant = new Pawn(PlayerColor.WHITE);
                    squares[11 - row, 11 - column].Occupant = new Pawn(PlayerColor.WHITE);
                }
            }
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
