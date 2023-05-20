using ChessAndAHalf.Data.Model;
using System;
using System.Collections.Generic;
using ChessAndAHalf.Data.Model.Pieces;
using ChessAndAHalf.Logic.Engine;
using System.Net.Sockets;

namespace ChessAndAHalf.Logic
{
    public class Game
    {
        public Board Board { get; private set; }
        public Square SelectedPiece { get; private set; }
        public PlayerColor currentPlayer { get; set; }
        private List<Pawn> enPassant = new List<Pawn>();
        private Square triggerEnPassant;
        private bool isEnPassant;
        public string message { get; set; }
        public Game()
        {
            Board = new Board();
            SelectedPiece = null;
            triggerEnPassant = null;
            currentPlayer = PlayerColor.WHITE;
        }

        static bool IsPortInUse(int port)
        {
            try
            {
                TcpListener listener = new TcpListener(System.Net.IPAddress.Any, port);
                listener.Start();
                listener.Stop();
                return false;
            }
            catch (SocketException)
            {
                return true;
            }
        }

        //Added just to see the functionality
        public void SelectPiece(int row, int column, bool red)
        {
            Square selectedSquare = Board.GetSquare(row, column);
            bool isHighlighted = selectedSquare.IsHighlighted;
            Board.ClearHighlight();
            Board.ClearCaptures();

            if (VerifyIfIsMyTurn(selectedSquare, red))
            {
                message += $"{row}#{column}#{red}";
                ClearEnPassant();
                if (red)
                {
                    if (isHighlighted)
                    {
                        triggerEnPassant.Occupant = null;   //captura piesa en passant
                    }
                    CapturePiece(selectedSquare);
                    ChangeTurn();
                    return;
                }

                List<Position> positions;

                if (selectedSquare != null && selectedSquare.Occupant != null)
                {
                    SelectedPiece = selectedSquare;
                    positions = selectedSquare.Occupant.GetLegalMoves(Board, selectedSquare);
                    positions = CheckDetector.FilterPositionsByCheck(positions, selectedSquare, Board);
                }
                else
                {
                    MovePiece(selectedSquare);
                    ChangeTurn();
                    return;
                }

                foreach (Position position in positions)
                {
                    message = $"{row}#{column}#{red}/";
                    if (selectedSquare.Occupant.Captures.Contains(position))
                    {
                        Board.GetSquare(position.Row, position.Column).IsCaptured = true;
                        if (triggerEnPassant != null && Board.GetSquare(position.Row, position.Column).Occupant == null)
                        {
                            Board.GetSquare(position.Row, position.Column).IsHighlighted = true;
                        }
                    }
                    else
                    {
                        Board.GetSquare(position.Row, position.Column).IsHighlighted = true;
                    }
                }
            }
        }
        public void MovePiece(Square selectedSquare)
        {
            VerifyEnPassant(selectedSquare);
            selectedSquare.Occupant = SelectedPiece.Occupant;
            SelectedPiece.Occupant = null;
            Type type = selectedSquare.Occupant.GetType();
            if (type.Equals(typeof(Knight)))
            {
                selectedSquare.Occupant.IsFirstMove = false;
            }
            VerifyPromotion(selectedSquare);
            
        }

        public void CapturePiece(Square selectedSquare)
        {
            Board.ClearCaptures();
                     
            selectedSquare.Occupant = SelectedPiece.Occupant;
            SelectedPiece.Occupant = null;
            VerifyPromotion(selectedSquare);
        }

        public void VerifyPromotion(Square selectedSquare)
        {
            if (selectedSquare.Occupant.Color == PlayerColor.WHITE)
            {
                if(selectedSquare.GetRow() == 0)
                {
                    Promotion(selectedSquare);
                }
            }
            else
            {
                if(selectedSquare.GetRow() == 11)
                {
                    Promotion(selectedSquare);
                }
            }
        }

        public void Promotion (Square selectedSquare)
        {
            Type type = selectedSquare.Occupant.GetType();
            if (type.Equals(typeof(Knight)))
            {
                selectedSquare.Occupant = new SpeedyKnight(selectedSquare.Occupant.Color);
            }
            if (type.Equals(typeof(Guard)))
            {
                selectedSquare.Occupant = new EquesRex(selectedSquare.Occupant.Color);
            }
            if (type.Equals(typeof(Pawn)))
            {
                //fereastra noua cu alegeri
                //pion promoveaza in Queen, Rook, Bishop, Knight, Cat or Guard 
                //selectedSquare.Occupant = new Alegere(selectedSquare.Occupant.Color);
            }

        }

        public bool VerifyIfIsMyTurn(Square selectedSquare, bool red)
        {
            return VerifyIfISelectMyPiece(selectedSquare) || VerifyIfIMoveMyPiece(selectedSquare) 
                || VerifyIfICapturePiece(red); 
        }
        public bool VerifyIfISelectMyPiece(Square selectedSquare)
        {
            return selectedSquare != null && selectedSquare.Occupant != null &&
                selectedSquare.Occupant.Color == currentPlayer;
        }

        public bool VerifyIfIMoveMyPiece(Square selectedSquare)
        {
            return selectedSquare != null && selectedSquare.Occupant == null &&
                SelectedPiece.Occupant.Color == currentPlayer;
        }
        public bool VerifyIfICapturePiece(bool red)
        {
            return red && SelectedPiece != null && SelectedPiece.Occupant != null
                && SelectedPiece.Occupant.Color == currentPlayer;
        }

        public void ChangeTurn()
        {
            if (CheckDetector.IsCheckMate(Board, currentPlayer) == true)
            {
                return;
            }

            if(currentPlayer == PlayerColor.WHITE)
            {
                currentPlayer = PlayerColor.BLACK;
            }
            else
            {
                currentPlayer = PlayerColor.WHITE;
            }
        }

        public void VerifyEnPassant(Square selectedSquare)
        {
            Type type = SelectedPiece.Occupant.GetType();
            Pawn pawn;
            Piece piece;
            if (type.Equals(typeof(Pawn)))
            {
                if ((Math.Abs(selectedSquare.GetRow() - SelectedPiece.GetRow()) == 2))
                {
                    piece = Board.GetSquare(selectedSquare.GetRow(), selectedSquare.GetColumn() + 1).Occupant;
                    if (piece != null && piece.GetType().Equals(typeof(Pawn)))
                    {
                        isEnPassant = true;
                        pawn = (Pawn)piece;
                        pawn.EnPassantLeft = true;
                        enPassant.Add(pawn);
                        triggerEnPassant = selectedSquare;
                    }
                    piece = Board.GetSquare(selectedSquare.GetRow(), selectedSquare.GetColumn() - 1).Occupant;
                    if (piece != null && piece.GetType().Equals(typeof(Pawn)))
                    {
                        isEnPassant = true;
                        pawn = (Pawn)piece;
                        pawn.EnPassantRight = true;
                        enPassant.Add(pawn);
                        triggerEnPassant = selectedSquare;
                    }
                }   
            }
        }

        public void ClearEnPassant()
        {
            if (triggerEnPassant != null && triggerEnPassant.Occupant != null
                    && triggerEnPassant.Occupant.Color == currentPlayer)
            {
                if (isEnPassant)
                {
                    isEnPassant = false;
                    foreach (Pawn pawn in enPassant)
                    {
                        pawn.EnPassantLeft = false;
                        pawn.EnPassantRight = false;
                    }
                    enPassant.Clear();
                }
            }
        }
    }
}
