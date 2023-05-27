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
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Window
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(((Button)sender).Name == "Multiplayer")
            {
                MainWindow main = new MainWindow("Multiplayer");
                this.Hide();
                main.Show();
            }
            else if(((Button)sender).Name =="Robot")
            {
                Difficulty difficulty = new Difficulty();
                this.Hide();
                difficulty.Show();
            }
        }
    }
}
