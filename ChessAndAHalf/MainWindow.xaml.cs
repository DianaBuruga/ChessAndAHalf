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
        const int SquareSize = 50;
        Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
        }

        private void DrawGameArea()
        {
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    Rectangle cell = new Rectangle
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                        Stroke = Brushes.Black,
                    };

                    if ((row + col) % 2 == 0)
                    {
                        cell.Fill = Brushes.White;
                    }
                    else
                    {
                        cell.Fill = Brushes.Gray;
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
                    pieceImage.Tag = $"{row}/{col}";

                    highlightImage.MouseDown += Image_MouseDown;
                    highlightImage.Tag = $"{row}/{col}";

                    Square square = game.Board.GetSquare(row, col);

                    Piece piece = square.Occupant;

                    if (piece != null)
                    {
                        pieceImage.Source = new BitmapImage(new Uri(piece.GetImagePath(), UriKind.Relative));
                    }

                    if (square.IsHighlighted)
                    {
                        highlightImage.Source = new BitmapImage(new Uri("../../Images/selectedSquare.png", UriKind.Relative));
                    }
                    if (square.IsCaptured)
                    {
                        cell.Fill = Brushes.Red;
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
            var image = sender as Image;
            string tag = image.Tag.ToString();
            string[] rowCol = tag.Split('/');
            int row = Int32.Parse(rowCol[0]);
            int col = Int32.Parse(rowCol[1]);
            game.SelectPiece(row, col);
            DrawGameArea();
        }
    }
}
