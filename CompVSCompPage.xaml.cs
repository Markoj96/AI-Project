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
    public partial class CompVSCompPage : Page
    {
        public CompVSCompPage()
        {
            InitializeComponent();
        }

        private void buttonHard_Click(object sender, RoutedEventArgs e)
        {
            int height1;
            int height2;
            try
            {
                height1 = Int32.Parse(heightValue1.Text);
                if (height1 == 0)
                    height1 = 1;
            }
            catch
            {
                height1 = 1;
            }
            
            try
            {
                height2 = Int32.Parse(heightValue2.Text);
                if (height2 == 0)
                    height2 = 1;
            }
            catch
            {
                height2 = 1;
            }

            Player player1 = new Easy(true, null, null, height1);
            Player player2 = new Easy(true, null, null, height2);
            GamePage gamePage = new GamePage(player1, player2);

            player1.GamePage = gamePage;
            player1.Figure1 = gamePage.Player1Figure1;
            player1.Figure2 = gamePage.Player1Figure2;
            player1.Opponent = player2;

            player2.GamePage = gamePage;
            player2.Figure1 = gamePage.Player2Figure1;
            player2.Figure2 = gamePage.Player2Figure2;
            player2.Opponent = player1;

            /*
                DISABLE BUTTONS SO HUMAN CANT CLICK IT
            */
            gamePage.disableAllButtons();
            
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(gamePage);
        }

        private void buttonMedium_Click(object sender, RoutedEventArgs e)
        {
            int height1;
            int height2;
            try
            {
                height1 = Int32.Parse(heightValue1.Text);
                if (height1 == 0)
                    height1 = 1;
            }
            catch
            {
                height1 = 1;
            }

            try
            {
                height2 = Int32.Parse(heightValue2.Text);
                if (height2 == 0)
                    height2 = 1;
            }
            catch
            {
                height2 = 1;
            }

            Player player1 = new Medium(true, null, null, height1);
            Player player2 = new Medium(true, null, null, height2);
            GamePage gamePage = new GamePage(player1, player2);

            player1.GamePage = gamePage;
            player1.Figure1 = gamePage.Player1Figure1;
            player1.Figure2 = gamePage.Player1Figure2;
            player1.Opponent = player2;

            player2.GamePage = gamePage;
            player2.Figure1 = gamePage.Player2Figure1;
            player2.Figure2 = gamePage.Player2Figure2;
            player2.Opponent = player1;

            /*
                DISABLE BUTTONS SO HUMAN CANT CLICK IT
            */
            gamePage.disableAllButtons();

            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(gamePage);
        }

        private void buttonEasy_Click(object sender, RoutedEventArgs e)
        {
            int height1;
            int height2;
            try
            {
                height1 = Int32.Parse(heightValue1.Text);
                if (height1 == 0)
                    height1 = 1;
            }
            catch
            {
                height1 = 1;
            }

            try
            {
                height2 = Int32.Parse(heightValue2.Text);
                if (height2 == 0)
                    height2 = 1;
            }
            catch
            {
                height2 = 1;
            }

            Player player1 = new Easy(true, null, null, height1);
            Player player2 = new Easy(true, null, null, height2);
            GamePage gamePage = new GamePage(player1, player2);

            player1.GamePage = gamePage;
            player1.Figure1 = gamePage.Player1Figure1;
            player1.Figure2 = gamePage.Player1Figure2;
            player1.Opponent = player2;

            player2.GamePage = gamePage;
            player2.Figure1 = gamePage.Player2Figure1;
            player2.Figure2 = gamePage.Player2Figure2;
            player2.Opponent = player1;

            /*
                DISABLE BUTTONS SO HUMAN CANT CLICK IT
            */
            gamePage.disableAllButtons();

            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(gamePage);
        }
    }
}
