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
    /// Interaction logic for Difficulty.xaml
    /// </summary>
    public partial class Difficulty : Window
    {
        public Difficulty()
        {
            InitializeComponent();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var difficulty = int.Parse(((ListBoxItem)sender).Tag.ToString());
            MainWindow mainWindow = new MainWindow("Robot", difficulty);
            mainWindow.Show();
        }
    }
}
