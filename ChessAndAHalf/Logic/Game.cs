using ChessAndAHalf.Data.Model;
using System.Collections.Generic;

namespace ChessAndAHalf.Logic
{
    public class Game
    {
        public Board Board { get; private set; }

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
                positions = selectedSquare.Occupant.GetLegalMoves(Board, selectedSquare);
            }
            else
            {
                return;
            }

            foreach (Position position in positions)
            {
                Board.GetSquare(position.Row, position.Column).IsHighlighted = true;
            }
        }
    }
}
