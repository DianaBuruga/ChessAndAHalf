using ChessAndAHalf.Data.Model;
using ChessAndAHalf.Logic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace ChessAndAHalf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isAIMoving { get; set; }

        string gameMode;
        const int SquareSize = 50;
        Client client;
        Game game;
        public PlayerColor MyColor { get; set; }
        public MainWindow(string gameMode, int difficulty = 0)
        {
            InitializeComponent();
            this.gameMode = gameMode;
            if (gameMode == "Multiplayer")
            {
                game = new Game(0, false, this);
                try
                {
                    client = new Client(this);
                }
                catch
                {
                    MessageBox.Show("Server is unavailable!");
                }
            }
            else
            {
                if(gameMode == "Robot")
                {
                    isAIMoving = false;
                    game = new Game(difficulty, true, this);
                }
            }
        }
        public void PrintMessage(string message)
        {
            MessageBox.Show(message);
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
        }

        public void DrawGameArea()
        {
            bool red;
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    red = false;
                    Rectangle cell = new Rectangle
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                        Stroke = Brushes.Black,
                    };

                    Square square = game.Board.GetSquare(row, col);
                    Piece piece = square.Occupant;

                    if ((row + col) % 2 == 0)
                    {
                        cell.Fill = Brushes.White;
                    }
                    else
                    {
                        cell.Fill = Brushes.Gray;
                    }
                    if (square.IsCaptured)
                    {
                        cell.Fill = Brushes.Red;
                        red = true;
                    }

                    Image pieceImage = new Image
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                    };

                    Image highlightImage = new Image
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                    };

                    pieceImage.MouseDown += Image_MouseDown;
                    pieceImage.Tag = $"{row}/{col}/{red}";

                    highlightImage.MouseDown += Image_MouseDown;
                    highlightImage.Tag = $"{row}/{col}/{red}";

                    
                    if (piece != null)
                    {
                        pieceImage.Source = new BitmapImage(new Uri(piece.GetImagePath(), UriKind.Relative));
                    }

                    if (square.IsHighlighted)
                    {
                        highlightImage.Source = new BitmapImage(new Uri("../../Images/selectedSquare.png", UriKind.Relative));
                    }

                    GameArea.Children.Add(cell);
                    GameArea.Children.Add(pieceImage);
                    GameArea.Children.Add(highlightImage);

                    Canvas.SetTop(cell, row * SquareSize);
                    Canvas.SetLeft(cell, col * SquareSize);
                    Canvas.SetTop(pieceImage, row * SquareSize);
                    Canvas.SetLeft(pieceImage, col * SquareSize);
                    Canvas.SetTop(highlightImage, row * SquareSize);
                    Canvas.SetLeft(highlightImage, col * SquareSize);
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(isAIMoving)
            {
                return;
            }

            var image = sender as Image;
            string tag = image.Tag.ToString();
            string[] rowColRed = tag.Split('/');
            int row = Int32.Parse(rowColRed[0]);
            int col = Int32.Parse(rowColRed[1]);
            bool red = Boolean.Parse(rowColRed[2]);
            if (client != null)
            {
                if (game.Board.currentPlayer == MyColor)
                {
                    Start(row, col, red);
                    if (game.Board.currentPlayer != MyColor)
                    {
                        client.SendMessages(game.message);
                        game.message = "";
                    }
                }
            }
            else
            {
                Start(row, col, red);
            }
        }
        public void Start(int row, int col, bool red)
        {
            game.SelectPiece(row, col, red);
            DrawGameArea();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.Disconnect();
            }
        }
        public void SetPromotion(string choose)
        {
            game.promotion = choose;
        }
    }
}
