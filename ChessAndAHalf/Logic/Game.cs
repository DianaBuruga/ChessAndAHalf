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
        public void SelectPiece(int row, int column, bool red)
        {
            Board.ClearHighlight();
            Board.ClearCaptures();
            Square selectedSquare = Board.GetSquare(row, column);
            if (red)
            {
                CapturePiece(selectedSquare);
                return;
            }
            
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
                if (selectedSquare.Occupant.Captures.Contains(position))
                {
                    Board.GetSquare(position.Row, position.Column).IsCaptured = true;
                }
                else
                {
                    Board.GetSquare(position.Row, position.Column).IsHighlighted = true;
                }
            }
        }
        public void MovePiece(Square selectedSquare)
        {
            selectedSquare.Occupant = SelectedPiece.Occupant;
            SelectedPiece.Occupant = null;
            Type type = selectedSquare.Occupant.GetType();
            if (type.Equals(typeof(ChessAndAHalf.Data.Model.Pieces.Knight)))
            {
                selectedSquare.Occupant.IsFirstMove = false;
            }
        }

        public void CapturePiece(Square selectedSquare)
        {
            Board.ClearCaptures();
         
            selectedSquare.Occupant = SelectedPiece.Occupant;
            SelectedPiece.Occupant = null;   
        }
    }
}
