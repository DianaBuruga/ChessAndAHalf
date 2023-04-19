using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model.Pieces
{
    public class StarCat : Piece
    {
        public StarCat(PlayerColor color) : base(color)
        {
        }

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteStarcat.png" : "../../Images/Pieces/blackStarcat.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            int currentRow = currentSquare.GetRow();
            int currentColumn = currentSquare.GetColumn();
            List<Position> legalMoves = new List<Position>();
            List<Position> captures = new List<Position>();

            int[,] directions = new int[,] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            for (int level = 1; level <= 3; level++)
            {
                for (int index = 0; index < 8; index++)
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
            }
            Captures = captures;
            return legalMoves;
        }
    }
}
