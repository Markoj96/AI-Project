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

namespace jm150635d
{
    /// <summary>
    /// Interaction logic for PVSCompPage.xaml
    /// </summary>
    public partial class PVSCompPage : Page
    {
        public PVSCompPage()
        {
            InitializeComponent();
        }

        private void buttonHard_Click(object sender, RoutedEventArgs e)
        {
            int height;
            try
            {
                height = Int32.Parse(heightValue.Text);
                if (height == 0)
                    height = 1;
            }
            catch
            {
                height = 1;
            }

            Player player1 = new Human();
            Player player2 = new Easy(true, null, null, height);
            GamePage gamePage = new GamePage(player1, player2);

            gamePage.playAllTurns.IsEnabled = false;

            player1.GamePage = gamePage;
            player1.Figure1 = gamePage.Player1Figure1;
            player1.Figure2 = gamePage.Player1Figure2;
            player1.Opponent = player2;

            player2.GamePage = gamePage;
            player2.Figure1 = gamePage.Player2Figure1;
            player2.Figure2 = gamePage.Player2Figure2;
            player2.Opponent = player1;
            
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(gamePage);
        }

        private void buttonMedium_Click(object sender, RoutedEventArgs e)
        {
            int height;
            try
            {
                height = Int32.Parse(heightValue.Text);
                if (height == 0)
                    height = 1;
            }
            catch
            {
                height = 1;
            }

            Player player1 = new Human();
            Player player2 = new Medium(true, null, null, height);
            GamePage gamePage = new GamePage(player1, player2);

            gamePage.playAllTurns.IsEnabled = false;

            player1.GamePage = gamePage;
            player1.Figure1 = gamePage.Player1Figure1;
            player1.Figure2 = gamePage.Player1Figure2;
            player1.Opponent = player2;

            player2.GamePage = gamePage;
            player2.Figure1 = gamePage.Player2Figure1;
            player2.Figure2 = gamePage.Player2Figure2;
            player2.Opponent = player1;
            
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(gamePage);
        }

        private void buttonEasy_Click(object sender, RoutedEventArgs e)
        {
            int height;
            try
            {
                height = Int32.Parse(heightValue.Text);
                if (height == 0)
                    height = 1;
            }
            catch
            {
                height = 1;
            }

            Player player1 = new Human();
            Player player2 = new Easy(true, null, null, height);
            GamePage gamePage = new GamePage(player1, player2);

            gamePage.playAllTurns.IsEnabled = false;

            player1.GamePage = gamePage;
            player1.Figure1 = gamePage.Player1Figure1;
            player1.Figure2 = gamePage.Player1Figure2;
            player1.Opponent = player2;

            player2.GamePage = gamePage;
            player2.Figure1 = gamePage.Player2Figure1;
            player2.Figure2 = gamePage.Player2Figure2;
            player2.Opponent = player1;
            
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(gamePage);
        }
    }
}
