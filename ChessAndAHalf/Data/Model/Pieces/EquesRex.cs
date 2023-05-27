using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model.Pieces
{
    public class EquesRex : Piece
    {
        public EquesRex(PlayerColor color) : base(color)
        {
        }

        public override int Points => 35;

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteEquesRex.png" : "../../Images/Pieces/blackEquesRex.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            int currentRow = currentSquare.GetRow();
            int currentColumn = currentSquare.GetColumn();
            List<Position> legalMoves = new List<Position>();
            List<Position> captures = new List<Position>();

            int[,] directions = new int[,] { { 2, -1 }, { 2, 1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }, { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            int level = 1;
            for (int index = 0; index < 16; index++)
            {
                Square square = board.GetSquare(currentRow + (directions[index, 0] * level), currentColumn + (directions[index, 1] * level));

                if (square != null)
                {
                    Position position = square.Position;
                    if (square.Occupant == null)
                    {
                        legalMoves.Add(position);
                    }
                    else if (square.Occupant != null)
                    {
                        if (square.Occupant.Color != Color)
                        {
                            legalMoves.Add(position);
                            captures.Add(position);
                        }
                    }
                }
            }
            Captures = captures;
            return legalMoves;
        }
    }
}
