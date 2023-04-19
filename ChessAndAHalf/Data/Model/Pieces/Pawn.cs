using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model.Pieces
{
    internal class Pawn : Piece
    {
        public bool EnPassantLeft { set; get; }
        public bool EnPassantRight { set; get; }
        public Pawn(PlayerColor color) : base(color)
        {

        }
        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whitePawn.png" : "../../Images/Pieces/blackPawn.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            int currentRow = currentSquare.GetRow();
            int currentColumn = currentSquare.GetColumn();
            List<Position> legalMoves = new List<Position>();

            int row = Color.Equals(PlayerColor.WHITE) ? -1 : 1;

            int[,] directions = new int[,] { { row, 0 }, { row, -1 }, { row, 1 } };

            int maxLevel = 0;
            int index = 0;
            maxLevel = isInFirstHalf(currentRow);

            for (int level = 1; level <= maxLevel; level++)
            {
                Square square = board.GetSquare(currentRow + (directions[index, 0] * level), currentColumn + (directions[index, 1] * level));

                if (square != null)
                {
                    if (square.Occupant == null)
                    {
                        Position position = square.Position;
                        legalMoves.Add(position);
                    }
                    else if (square.Occupant != null)
                    {
                        /*Position position = square.Position;
                        legalMoves.Add(position);
                        cand se ia piesa ar trebui sa fie ocupat*/
                        break;
                    }
                }
            }
            /*if (EnPassantLeft)
            {
                int level = 1;
                index = 1;
                Square square = board.GetSquare(currentRow + (directions[index, 0] * level), currentColumn + (directions[index, 1] * level));

                if (square != null && square.Occupant == null)
                {
                    Position position = square.Position;
                    legalMoves.Add(position);
                }
            }
            if (EnPassantRight)
            {
                int level = 1;
                index = 2;
                Square square = board.GetSquare(currentRow + (directions[index, 0] * level), currentColumn + (directions[index, 1] * level));

                if (square != null && square.Occupant == null)
                {
                    Position position = square.Position;
                    legalMoves.Add(position);
                }
            }*/
            return legalMoves;
        }
        private int isInFirstHalf(int currentRow)
        {
            int maxLevel = 0;
            switch (Color)
            {
                case PlayerColor.BLACK:
                    if (currentRow < 5)
                    {
                        maxLevel = 5 - currentRow;
                    }
                    else
                    {
                        maxLevel = 1;
                    }
                    break;
                case PlayerColor.WHITE:
                    if (currentRow > 6)
                    {
                        maxLevel = currentRow - 6;
                    }
                    else
                    {
                        maxLevel = 1;
                    }
                    break;
            }
            return maxLevel;
        }
    }
}

