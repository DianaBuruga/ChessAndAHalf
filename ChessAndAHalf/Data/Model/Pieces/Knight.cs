﻿using System.Collections.Generic;

namespace ChessAndAHalf.Data.Model.Pieces
{
    internal class Knight : Piece
    {
        public bool isFirstMove;
        public Knight(PlayerColor color) : base(color)
        {
            isFirstMove = true;
        }
        public override string GetImagePath()
        {
            return Color == PlayerColor.WHITE ? "../../Images/Pieces/whiteKnight.png" : "../../Images/Pieces/blackKnight.png";
        }

        public override List<Position> GetLegalMoves(Board board, Square currentSquare)
        {
            int currentRow = currentSquare.GetRow();
            int currentColumn = currentSquare.GetColumn();
            List<Position> legalMoves = new List<Position>();

            int[,] directions = new int[,] { { 2, -1 }, { 2, 1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };
            int maxLevel = 1;
            if (isFirstMove)
            {
                maxLevel = 2;// vezi ca dupa prima mutare trebuie sa faci isFirstMove pe false;
            }
            for (int index = 0; index < 8; index++)
            {
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
            }
            return legalMoves;
        }
        public List<Position> ComputeLegalMoves(Board board, int currentRow, int currentColumn)
        {
            List<Position> legalMoves = new List<Position>();
            int[,] directions = new int[,] { { 2, -1 }, { 2, 1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };
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
