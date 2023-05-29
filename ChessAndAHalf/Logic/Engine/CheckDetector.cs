using ChessAndAHalf.Data.Model;
using ChessAndAHalf.Data.Model.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAndAHalf.Logic.Engine
{
    public class CheckDetector
    {
        public static bool IsKingInCheck(Board board, PlayerColor player)
        {

            Position kingPosition = board.GetKingPosition(player);
            PlayerColor opponentPlayer = (player == PlayerColor.WHITE) ? PlayerColor.BLACK : PlayerColor.WHITE;

            foreach (Square squareWithPiece in board.GetSquaresWithPiece(opponentPlayer))
            {
                foreach (Position position in squareWithPiece.Occupant.GetLegalMoves(board, squareWithPiece))
                {
                    if (position.Row == kingPosition.Row && position.Column == kingPosition.Column)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<Position> FilterPositionsByCheck(List<Position> positions, Square initialSquare, Board board) { 
            List<Position> result = new List<Position>();

            Board clonedBoard = board.CloneBoard();
            foreach (Position position in positions)
            {
                if(IsGoodPosition(position, initialSquare, clonedBoard) == true)
                    result.Add(position);
            }

            return result;
        }

        private static bool IsGoodPosition(Position position, Square initialSquare, Board board)
        {
            bool result; ;
            PlayerColor playerColor = initialSquare.Occupant.Color;
            Position initialPosition = new Position(initialSquare.GetRow(), initialSquare.GetColumn());
            Move move = new Move(initialPosition, position);
            Piece removedPiece = DoMove(board, move);

            if(IsKingInCheck(board, playerColor))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            UndoMove(board, move, removedPiece);

            return result;
        }

        private static Piece DoMove(Board board, Move move)
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

        private static void UndoMove(Board board, Move move, Piece removedPiece)
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

        public static bool IsCheckMate(Board board, PlayerColor player)
        {
            bool result = true;
            PlayerColor opponentPlayer = (player == PlayerColor.WHITE) ? PlayerColor.BLACK : PlayerColor.WHITE;

            foreach (Square squareWithPiece in board.GetSquaresWithPiece(opponentPlayer))
            {
                List<Position> positions = squareWithPiece.Occupant.GetLegalMoves(board, squareWithPiece);
                positions = FilterPositionsByCheck(positions, squareWithPiece, board);
                if(positions.Count > 0)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
