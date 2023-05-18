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

            foreach (Position position in positions)
            {
                if(IsGoodPosition(position, initialSquare, board) == true)
                    result.Add(position);
            }

            return result;
        }

        private static bool IsGoodPosition(Position position, Square initialSquare, Board board)
        {
            bool result; ;
            PlayerColor playerColor = initialSquare.Occupant.Color;
            Position initialPosition = new Position(initialSquare.GetRow(), initialSquare.GetColumn());
            Piece piece = initialSquare.Occupant;
            Board clonedBoard = board.CloneBoard();

            clonedBoard.RemovePiece(initialPosition);
            clonedBoard.AddPiece(position, piece);

            if(IsKingInCheck(clonedBoard, playerColor))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            clonedBoard.RemovePiece(position);
            clonedBoard.AddPiece(initialPosition, piece);

            return result;
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
