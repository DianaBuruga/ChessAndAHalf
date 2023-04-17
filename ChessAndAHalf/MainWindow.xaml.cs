using ChessAndAHalf.Data.Model;
using ChessAndAHalf.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
        }

        private void DrawGameArea()
        {
            Game game = new Game();
            for (int row = 0; row < game.Board.Size; row++)
            {
                for (int col = 0; col < game.Board.Size; col++)
                {
                    Rectangle square = new Rectangle
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                        Stroke = Brushes.Black,
                    };
                
                    if ((row + col) % 2 == 0)   
                    {
                        square.Fill = Brushes.White;
                    }
                    else
                    {
                        square.Fill = Brushes.Gray;
                    }

                    Image image = new Image
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                    };

                    image.MouseDown += Image_MouseDown;

                    Piece piece = game.Board.GetSquare(row, col).Occupant;
                    if (piece != null)
                    {
                        image.Source = new BitmapImage(new Uri(piece.GetImagePath(), UriKind.Relative));
                    }

                    GameArea.Children.Add(square);
                    GameArea.Children.Add(image);

                    Canvas.SetTop(square, row * SquareSize);
                    Canvas.SetLeft(square, col * SquareSize);
                    Canvas.SetTop(image, row * SquareSize);
                    Canvas.SetLeft(image, col * SquareSize);
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            image.Source = new BitmapImage(new Uri("../../Images/Pieces/whiteStarcat.png", UriKind.Relative));
        }
    }
}
