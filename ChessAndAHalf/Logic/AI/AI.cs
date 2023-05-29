using ChessAndAHalf.Data.Model;
using ChessAndAHalf.Data.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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
            {
                return CalculatePoint(board);
            }

            List<Move> possibleMoves = isMaximizingPlayer ? board.GetAllLegalMoves(PlayerColor.BLACK) : board.GetAllLegalMoves(PlayerColor.WHITE);

            int bestValue = isMaximizingPlayer ? int.MinValue : int.MaxValue;

            List<Move> orderedMoves = possibleMoves.OrderByDescending(move => GetMoveOrderingHeuristic(move, board)).ToList();

            foreach (var move in orderedMoves)
            {
                Piece removedPiece = DoMove(board, move);

                int value = Minimax(board, depth - 1, alpha, beta, !isMaximizingPlayer);

                UndoMove(board, move, removedPiece);

                if (isMaximizingPlayer)
                {
                    bestValue = Math.Max(value, bestValue);
                    alpha = Math.Max(alpha, value);
                }
                else
                {
                    bestValue = Math.Min(value, bestValue);
                    beta = Math.Min(beta, value);
                }

                if (beta <= alpha)
                {
                    break;
                }
            }

            return bestValue;
        }

        int GetMoveOrderingHeuristic(Move move, Board board)
        {
            int moveScore = 0;

            if (board.GetSquare(move.Next.Row, move.Next.Column).Occupant != null)
            {
                moveScore += 10 * GetPieceValue(board, move.Next) - GetPieceValue(board, move.Tile);
            }

            return moveScore;
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
            
            Board newBoard = board.CloneBoard();

            foreach (var move in possibleMoves)
            {
                Piece removedPiece = DoMove(newBoard, move);

                int value = Minimax(newBoard, _depth, int.MinValue, int.MaxValue, turn);

                if (value >= bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }

                UndoMove(newBoard, move, removedPiece);
            }

            return bestMove;
        }

        private Piece DoMove(Board board, Move move)
        {
            Piece removedPiece = null;

            Square sourceSquare = board.GetSquare(move.Tile.Row, move.Tile.Column);
            Square targetSquare = board.GetSquare(move.Next.Row, move.Next.Column);

            if (targetSquare.Occupant != null)
            {
                removedPiece = targetSquare.Occupant;
            }

            Piece movingPiece = sourceSquare.Occupant;
            board.RemovePiece(move.Tile);
            board.AddPiece(move.Next, movingPiece);

            return removedPiece;
        }

        private void UndoMove(Board board, Move move, Piece removedPiece)
        {
            Square sourceSquare = board.GetSquare(move.Next.Row, move.Next.Column);

            Piece movingPiece = sourceSquare.Occupant;
            board.RemovePiece(move.Next);
            board.AddPiece(move.Tile, movingPiece);

            if (removedPiece != null)
            {
                board.AddPiece(move.Next, removedPiece);
            }
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
