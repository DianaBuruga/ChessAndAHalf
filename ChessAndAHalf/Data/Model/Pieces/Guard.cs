using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model.Pieces
{
    class Guard : Piece
    {
        public Guard(PlayerColor color) : base(color)
        {
        }

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteGuard.png" : "../../Images/Pieces/blackGuard.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            int currentRow = currentSquare.GetRow();
            int currentColumn = currentSquare.GetColumn();
            List<Position> legalMoves = new List<Position>();

            int[,] directions = new int[,] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            int level = 1;
            for (int index = 0; index < 8; index++)
            {
                Square square = board.GetSquare(currentRow + (directions[index, 0] * level), currentColumn + (directions[index, 1] * level));

                if (square != null && square.Occupant == null)
                {
                    Position position = square.Position;
                    legalMoves.Add(position);
                }
            }

            return legalMoves;
        }
    }
}
