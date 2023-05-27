using ChessAndAHalf.Data.Model;
using ChessAndAHalf.Data.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Logic
{
    public class AI
    {
        private int _depth;

        public AI(int depth)
        {
            _depth = depth;
        }

        private Board GenerateMovedBoard(Board oldBoard, Move move)
        {
            Board newBoard = new Board();
            newBoard = oldBoard.CloneBoard();
            newBoard.RemovePiece(move.Tile);
            newBoard.AddPiece(move.Next, oldBoard.GetSquare(move.Tile.Row, move.Tile.Column).Occupant);
            return newBoard;
        }

        private int Minimax(Board board, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            if (depth == 0)
                return CalculatePoint(board);

            if (isMaximizingPlayer)
            {
                int bestValue = int.MinValue;

                List<Move> possibleMoves = board.GetAllLegalMoves(PlayerColor.BLACK);

                foreach (var move in possibleMoves)
                {
                    Board newBoard = GenerateMovedBoard(board, move);

                    int value = Minimax(newBoard, depth - 1, alpha, beta, false);

                    bestValue = Math.Max(value, bestValue);

                    alpha = Math.Max(alpha, value);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return bestValue;
            }
            else
            {
                int bestValue = int.MaxValue;

                List<Move> possibleMoves = board.GetAllLegalMoves(PlayerColor.WHITE);

                foreach (var move in possibleMoves)
                {
                    Board newBoard = GenerateMovedBoard(board, move);

                    int value = Minimax(board, depth - 1, alpha, beta, true);

                    bestValue = Math.Min(value, bestValue);

                    beta = Math.Min(beta, value);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return bestValue;
            }
        }


        public Move GetBestMove(Board board)
        {
            int bestValue = int.MinValue;
            Move bestMove = null;
            bool turn;
            if (board.currentPlayer == PlayerColor.WHITE)
            {
                turn = false;
            }
            else
            {
                turn = true;
            }

            List<Move> possibleMoves = board.GetAllLegalMoves(board.currentPlayer);

            foreach (var move in possibleMoves)
            {
                Board newBoard = GenerateMovedBoard(board, move);

                int value = Minimax(newBoard, _depth, int.MinValue, int.MaxValue, turn);

                if (value >= bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }

            return bestMove;
        }


        private int GetPieceValue(Board board, Position position)
        {
            Square square = board.GetSquare(position.Row, position.Column);
            if(square.Occupant != null)
            {
                return square.Occupant.Points;
            }

            return 0;
        }

        public int CalculatePoint(Board board)
        {
            int scoreWhite = 0;
            int scoreBlack = 0;
            scoreWhite += GetScoreFromExistingPieces(PlayerColor.WHITE, board);
            scoreBlack += GetScoreFromExistingPieces(PlayerColor.BLACK, board);

            int evaluation = scoreBlack - scoreWhite;

            int prespective = (board.currentPlayer == PlayerColor.BLACK) ? -1 : 1;
            return evaluation * prespective;
        }

        private static int GetScoreFromExistingPieces(PlayerColor palyerColor, Board board)
        {
            int material = 0;
            List<Square> squaresWithPiece = board.GetSquaresWithPiece(palyerColor);

            foreach (Square square in squaresWithPiece)
            {
                material += square.Occupant.Points;
            }

            return material;
        }
    }
}
