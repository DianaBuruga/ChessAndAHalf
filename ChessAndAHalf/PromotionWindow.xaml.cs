using ChessAndAHalf.Data.Model;
using ChessAndAHalf.Data.Model.Pieces;
using ChessAndAHalf.Logic;
using ChessAndAHalf.Shared;
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
using System.Windows.Shapes;

namespace ChessAndAHalf
{
    /// <summary>
    /// Interaction logic for PromotionWindow.xaml
    /// </summary>
    public partial class PromotionWindow : Window
    {
        const int SquareSize = 60;
        private PlayerColor ReceivedColor;
        public string promotionPiece { get; private set; }
        public PromotionWindow()
        {
            InitializeComponent();
        }

        public PromotionWindow(PlayerColor playerColor)
        {
            InitializeComponent();
            ReceivedColor = playerColor;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawPromotionArea(ReceivedColor);
        }

        private void DrawPromotionArea(PlayerColor playerColor)
        {
            string[] piecesImage = playerColor == PlayerColor.WHITE
                    ? new string[] { "../../Images/Pieces/whiteKnight.png", "../../Images/Pieces/whiteCat.png", "../../Images/Pieces/whiteGuard.png", "../../Images/Pieces/whiteRook.png", "../../Images/Pieces/whiteBishop.png", "../../Images/Pieces/whiteQueen.png" }
                    : new string[] { "../../Images/Pieces/blackKnight.png", "../../Images/Pieces/blackCat.png", "../../Images/Pieces/blackGuard.png", "../../Images/Pieces/blackRook.png", "../../Images/Pieces/blackBishop.png", "../../Images/Pieces/blackQueen.png" };
            string[] piece = new string[] { "Knight", "Cat", "Guard", "Rook", "Bishop", "Queen" };

            int index = 0; 
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Rectangle cell = new Rectangle
                    {
                        Width = SquareSize,
                        Height = SquareSize,
                        Stroke = Brushes.Black,
                    };

                    Image pieceImage = new Image
                    {
                        Width = SquareSize,
                        Height = SquareSize,

                    };

                    pieceImage.Source = new BitmapImage(new Uri(piecesImage[index], UriKind.Relative));
                    pieceImage.Tag = $"{piece[index]}";
                    pieceImage.MouseDown += Image_MouseDown;

                    PromotionBoard.Children.Add(cell);
                    PromotionBoard.Children.Add(pieceImage);
                    Canvas.SetTop(cell, row * SquareSize);
                    Canvas.SetLeft(cell, col * SquareSize);
                    Canvas.SetTop(pieceImage, row * SquareSize);
                    Canvas.SetLeft(pieceImage, col * SquareSize);
                    index++;
                }
            }

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            string tag = image.Tag.ToString();
            promotionPiece = tag;
            this.Close();
        }
    }
}
