using ChessAndAHalf.Data.Model;
using System;
using System.Collections.Generic;

namespace ChessAndAHalf.Logic
{
    public class Game
    {
        public Board Board { get; private set; }
        public Square SelectedPiece { get; private set; }
        public Game()
        {
            Board = new Board();
        }

        //Added just to see the functionality
        public void SelectPiece(int row, int column)
        {
            Board.ClearHighlight();

            Square selectedSquare = Board.GetSquare(row, column);
            List<Position> positions;

            if (selectedSquare != null && selectedSquare.Occupant != null)
            {
                SelectedPiece = selectedSquare;
                positions = selectedSquare.Occupant.GetLegalMoves(Board, selectedSquare);
            }
            else
            {
                MovePiece(selectedSquare);
                return;
            }

            foreach (Position position in positions)
            {
                Board.GetSquare(position.Row, position.Column).IsHighlighted = true;
            }
        }
        public void MovePiece(Square selectedSquare)
        {
            selectedSquare.Occupant = SelectedPiece.Occupant;
            SelectedPiece.Occupant = null;
            Type t = selectedSquare.Occupant.GetType();
            if (t.Equals(typeof(ChessAndAHalf.Data.Model.Pieces.Knight)))
            {
                selectedSquare.Occupant.IsFirstMove = false;
            }
        }
    }
}
