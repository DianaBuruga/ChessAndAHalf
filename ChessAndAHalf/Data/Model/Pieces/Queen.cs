﻿using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model.Pieces
{
    public class Queen : Piece
    {
        public Queen(PlayerColor color) : base(color)
        {

        }

        public override int Points => 90;

        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteQueen.png" : "../../Images/Pieces/blackQueen.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            int currentRow = currentSquare.GetRow();
            int currentColumn = currentSquare.GetColumn();
            List<Position> legalMoves = new List<Position>();
            List<Position> captures = new List<Position>();

            int[,] directions = new int[,] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            for (int index = 0; index < 8; index++)
            {
                for (int level = 1; level <= 11; level++)
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
                            break;
                        }
                    }
                }
            }
            Captures = captures;
            return legalMoves;
        }
    }
}
