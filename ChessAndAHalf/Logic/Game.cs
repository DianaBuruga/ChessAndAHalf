using ChessAndAHalf.Data.Model;
using System;
using System.Collections.Generic;
using ChessAndAHalf.Data.Model.Pieces;
using ChessAndAHalf.Logic.Engine;
using System.Net.Sockets;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChessAndAHalf.Logic
{
    public class Game
    {
        public Board Board { get; private set; }
        public Square SelectedPiece { get; private set; }
        private List<Pawn> EnPassant = new List<Pawn>();
        private Square TriggerEnPassant;
        private bool IsEnPassant;
        public string message { get; set; }
        private bool IsAI;
        private AI AIPlayer;
        private MainWindow mainWindow;

        public Game(int difficulty, bool isAI, MainWindow mainWindow)
        {
            Board = new Board();
            SelectedPiece = null;
            TriggerEnPassant = null;
            IsAI = isAI;
            AIPlayer = new AI(difficulty);
            this.mainWindow = mainWindow;
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
                        TriggerEnPassant.Occupant = null;   //captura piesa en passant
                    }
                    CapturePiece(selectedSquare);
                    if (IsAI)
                    {
                        Task computationTask = Task.Run(() => PerformAIComputation());
                    }
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

                    if (IsAI)
                    {
                        Task computationTask = Task.Run(() => PerformAIComputation());
                    }

                    ChangeTurn();
                    return;
                }

                foreach (Position position in positions)
                {
                    message = $"{row}#{column}#{red}/";
                    if (selectedSquare.Occupant.Captures.Contains(position))
                    {
                        Board.GetSquare(position.Row, position.Column).IsCaptured = true;
                        if (TriggerEnPassant != null && Board.GetSquare(position.Row, position.Column).Occupant == null)
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

        public async Task PerformAIComputation()
        {
            mainWindow.isAIMoving = true;
            Board.currentPlayer = PlayerColor.BLACK;
            Move move = AIPlayer.GetBestMove(Board);
            SelectedPiece = Board.GetSquare(move.Tile.Row, move.Tile.Column);
            MovePiece(Board.GetSquare(move.Next.Row, move.Next.Column));

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                mainWindow.DrawGameArea();
                mainWindow.isAIMoving = false;
            });
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
                PromotionWindow promotionWindow = new PromotionWindow(Board.currentPlayer);
                bool? dialogResult = promotionWindow.ShowDialog();
                string info;

                if (dialogResult == true)
                {
                    info = promotionWindow.promotionPiece;
                }
                else
                {
                    info = promotionWindow.promotionPiece;
                }

                switch (info)
                {
                    case "Knight":
                        selectedSquare.Occupant = new Knight(selectedSquare.Occupant.Color);
                        break;
                    case "Cat":
                        selectedSquare.Occupant = new Cat(selectedSquare.Occupant.Color);
                        break;
                    case "Guard":
                        selectedSquare.Occupant = new Guard(selectedSquare.Occupant.Color);
                        break;
                    case "Rook":
                        selectedSquare.Occupant = new Rook(selectedSquare.Occupant.Color);
                        break;
                    case "Bishop":
                        selectedSquare.Occupant = new Bishop(selectedSquare.Occupant.Color);
                        break;
                    case "Queen":
                        selectedSquare.Occupant = new Queen(selectedSquare.Occupant.Color);
                        break;
                    default: 
                        break;
                }
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
                selectedSquare.Occupant.Color == Board.currentPlayer;
        }

        public bool VerifyIfIMoveMyPiece(Square selectedSquare)
        {
            return selectedSquare != null && selectedSquare.Occupant == null &&
                SelectedPiece.Occupant.Color == Board.currentPlayer;
        }
        public bool VerifyIfICapturePiece(bool red)
        {
            return red && SelectedPiece != null && SelectedPiece.Occupant != null
                && SelectedPiece.Occupant.Color == Board.currentPlayer;
        }

        public void ChangeTurn()
        {
            if (CheckDetector.IsCheckMate(Board, Board.currentPlayer) == true)
            {
                return;
            }

            if(Board.currentPlayer == PlayerColor.WHITE)
            {
                Board.currentPlayer = PlayerColor.BLACK;
            }
            else
            {
                Board.currentPlayer = PlayerColor.WHITE;
            }
        }

        public void VerifyEnPassant(Square selectedSquare)
        {
            Type type = SelectedPiece.Occupant.GetType();
            Pawn pawn;
            Piece piece;
            Square square;
            if (type.Equals(typeof(Pawn)))
            {
                if (Math.Abs(selectedSquare.GetRow() - SelectedPiece.GetRow()) <=4 && Math.Abs(selectedSquare.GetRow() - SelectedPiece.GetRow())>1)
                {
                    square = Board.GetSquare(selectedSquare.GetRow(), selectedSquare.GetColumn() + 1);
                    if (square != null) { 
                        piece=square.Occupant;
                        if (piece != null && piece.GetType().Equals(typeof(Pawn)))
                        {
                            IsEnPassant = true;
                            pawn = (Pawn)piece;
                            pawn.EnPassantLeft = true;
                            EnPassant.Add(pawn);
                            TriggerEnPassant = selectedSquare;
                        }
                    }
                    square = Board.GetSquare(selectedSquare.GetRow(), selectedSquare.GetColumn() - 1);
                    if (square != null)
                    {
                        piece = square.Occupant;
                        if (piece != null && piece.GetType().Equals(typeof(Pawn)))
                        {
                            IsEnPassant = true;
                            pawn = (Pawn)piece;
                            pawn.EnPassantRight = true;
                            EnPassant.Add(pawn);
                            TriggerEnPassant = selectedSquare;
                        }
                    }
                }
            }
        }

        public void ClearEnPassant()
        {
            if (TriggerEnPassant != null && TriggerEnPassant.Occupant != null
                    && TriggerEnPassant.Occupant.Color == Board.currentPlayer)
            {
                if (IsEnPassant)
                {
                    IsEnPassant = false;
                    foreach (Pawn pawn in EnPassant)
                    {
                        pawn.EnPassantLeft = false;
                        pawn.EnPassantRight = false;
                    }
                    EnPassant.Clear();
                }
            }
        }
    }
}
