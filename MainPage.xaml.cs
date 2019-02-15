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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonPVSP_Click(object sender, RoutedEventArgs e)
        {
            Player player1 = new Human();
            Player player2 = new Human();
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
                Disable buttons beacuse it is Player vs Player mode
            */
            gamePage.playOneTurn.IsEnabled = false;
            gamePage.playAllTurns.IsEnabled = false;

            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(gamePage);
        }

        private void buttonPVSComp_Click(object sender, RoutedEventArgs e)
        {
            PVSCompPage pagePVSComp = new PVSCompPage();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(pagePVSComp);
        }
        
        private void buttonCompVSComp_Click(object sender, RoutedEventArgs e)
        {
            CompVSCompPage pageCompVSComp = new CompVSCompPage();
            NavigationService ns = NavigationService.GetNavigationService(this);
            ns.Navigate(pageCompVSComp);
        }
        
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
