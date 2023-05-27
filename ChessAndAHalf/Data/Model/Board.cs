using ChessAndAHalf.Data.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ChessAndAHalf.Data.Model
{
    public class Board
    {
        private Square[,] squares;
        public PlayerColor currentPlayer { get; set; }
        public int Size { get; set; }

        public Board()
        {
            currentPlayer = PlayerColor.WHITE;
            Size = 12;

            squares = new Square[Size, Size];

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    squares[row, col] = new Square(row, col);
                }
            }
            StartPlacement();
        }

        public void StartPlacement()
        {
            int column = 0, row = 0;
            squares[row, column].Occupant = new Rook(PlayerColor.BLACK);
            squares[row, 11 - column].Occupant = new Rook(PlayerColor.BLACK);
            squares[11, column].Occupant = new Rook(PlayerColor.WHITE);
            squares[11, 11 - column].Occupant = new Rook(PlayerColor.WHITE);
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
            for (column = 0; column < 6; column++)
            {
                if (column == 3)
                {
                    squares[row, column].Occupant = new Guard(PlayerColor.BLACK);
                    squares[row, 11 - column].Occupant = new Guard(PlayerColor.BLACK);
                    squares[11 - row, column].Occupant = new Guard(PlayerColor.WHITE);
                    squares[11 - row, 11 - column].Occupant = new Guard(PlayerColor.WHITE);
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

        public void ClearCaptures()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    squares[row, col].IsCaptured = false;
                }
            }
        }

        public Position GetKingPosition(PlayerColor playerColor)
        {
            foreach (Square square in squares)
            {
                if (square.Occupant is King && square.Occupant.Color == playerColor)
                {
                    Position position = square.Position;
                    return position;
                }
            }
            return null;
        }

        public List<Square> GetSquaresWithPiece(PlayerColor playerColor)
        {
            List<Square> pieceSquares = new List<Square>();

            foreach (Square square in squares)
            {
                if (square.Occupant != null && square.Occupant.Color == playerColor)
                {
                    pieceSquares.Add(square);
                }
            }

            return pieceSquares;
        }

        public void RemovePiece(Position position)
        {
            squares[position.Row, position.Column].Occupant = null;
        }

        public void AddPiece(Position position, Piece piece)
        {
            squares[position.Row, position.Column].Occupant = piece;
        }

        public Board CloneBoard()
        {
            Board clonedboard = new Board
            {
                Size = Size
            };

            foreach (Square square in squares)
            {
                if (square.Occupant == null)
                {
                    Square clonedSquare = new Square(square.GetRow(), square.GetColumn());
                    clonedboard.squares[square.GetRow(), square.GetColumn()] = clonedSquare;
                }
                else
                {
                    Type originalType = square.Occupant.GetType();
                    Square clonedSquare = new Square(square.GetRow(), square.GetColumn(), (Piece)Activator.CreateInstance(originalType, square.Occupant.Color));
                    clonedboard.squares[square.GetRow(), square.GetColumn()] = clonedSquare;
                }
            }

            return clonedboard;
        }

        public List<Move> GetAllLegalMoves(PlayerColor playerColor, bool filter = true)
        {
            List<Move> allLegalMoves = new List<Move>();
            List<Square> allSquaresWithPieces = GetSquaresWithPiece(playerColor);

            foreach (var square in allSquaresWithPieces)
            {
                List<Position> legalMoves = square.Occupant.GetLegalMoves(this, square);
                legalMoves.AddRange(square.Occupant.Captures);

                foreach (var move in legalMoves)
                {
                    allLegalMoves.Add(new Move(square.Position, move));
                }
            }

            return allLegalMoves;
        }
    }
}
