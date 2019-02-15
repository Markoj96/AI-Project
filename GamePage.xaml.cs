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
using jm150635d;

namespace jm150635d
{
    public partial class GamePage : Page
    {
        /*
            Array of Fields, contain values of plots on field
        */
        public int[,] fields;

        /*
            Array of Grids, contain content for button
        */
        public Grid[,] grids;

        /*
            Array of Buttons, main content of the game, button represents the field
        */
        public Button[,] buttons;

        /*
            Array of TextBlock, represents the value of plots on field
        */
        public TextBlock[,] texts;

        /*
            Array of Ellipses, represents blocked fields
        */
        public Ellipse[,] ellipses;

        /*
            Array of Boolean for turrets, represents if ellipse is placed
        */
        public bool[,] turrets;

        /*
            Modes in game.
            1 - Setting figures on field
            2.1 - Selecting figure to move
            2.2 - Moving
            2.3 - Building with same figure
        */
        public bool settingFigureMode;
        public bool movingMode;
        public bool buildingMode;
        public bool selectingFigure;

        /*
            Players in the game, their states and figure positions.
            player1, player2 => players
            readyPlayer1, readyPlayer2 -> states, they are ready if they setted figures on field
            players => array for player figures
                0, 0 - X axis of first figure of first player
                0, 0 - Y axis of first figure of first player
                0, 2 - X axis of second figure of first player
                0, 3 - Y axis of second figure of first player
                1, 0 - X axis of first figure of first player
                1, 0 - Y axis of first figure of first player
                1, 2 - X axis of second figure of first player
                1, 3 - Y axis of second figure of first player
        */
        public Player player1;
        public Player player2;
        public bool readyPlayer1;
        public bool readyPlayer2;
        public int[,] players;
        public int currentPlayer;

        /*
            index of X and Y in players array
        */
        public int currentFigureX;
        public int currentFigureY;

        /*
            Current figure, after selecting
        */
        public UIElement currentFigure;

        /*
            possibleMoves where player can move figure. Indicator for end of the game
            possibleMoves = 0 -> end of the game
                    */
        public int possibleMoves;
        public bool gameEnded;
        public bool GameEnded
        {
            get
            {
                return gameEnded;
            }
            set
            {
                gameEnded = value;
            }
        }

        public GamePage(Player p1, Player p2)
        {
            InitializeComponent();

            currentPlayer = 1;

            settingFigureMode = true;
            movingMode = false;
            buildingMode = false;
            selectingFigure = false;

            player1 = p1;
            player2 = p2;
            readyPlayer1 = false;
            readyPlayer2 = false;

            players = new int[2, 4];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                    players[i, j] = -1;
            
            fields = new int[5, 5];
            turrets = new bool[5, 5];
            texts = new TextBlock[5, 5];
            ellipses = new Ellipse[5, 5];
            grids = new Grid[5, 5];
            buttons = new Button[5, 5];

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    fields[i, j] = 0;

                    turrets[i, j] = false;

                    texts[i, j] = new TextBlock();
                    texts[i, j].Text = "0";
                    texts[i, j].FontSize = 32.0;
                    texts[i, j].Name = "Text" + i + j;

                    ellipses[i, j] = new Ellipse();
                    ellipses[i, j].Fill = Brushes.Black;
                    ellipses[i, j].Opacity = 0.0;
                    ellipses[i, j].Name = "Ellipse" + i + j;

                    grids[i, j] = new Grid();
                    grids[i, j].Children.Add(texts[i, j]);
                    grids[i, j].Children.Add(ellipses[i, j]);
                    grids[i, j].Name = "Grid" + i + j;

                    buttons[i, j] = new Button();
                    buttons[i, j].Background = (Brush)(new BrushConverter().ConvertFromString("#FFC8C8C8"));
                    buttons[i, j].BorderBrush = Brushes.Black;
                    buttons[i, j].BorderThickness = new Thickness(3);
                    buttons[i, j].Click += buttonClicked;
                    buttons[i, j].Name = "Button" + i + j;
                    buttons[i, j].Content = grids[i, j];
                    
                    Grid.SetRow(buttons[i, j], i);
                    Grid.SetColumn(buttons[i, j], j);
                    mainGrid.Children.Add(buttons[i, j]);
                }
            }

            turnMode.Text = "Setting figures";
            winnerInfo.Text = "";
            winner.Text = "";
        }

        /*
            This method returns current player
        */
        public Player getCurrentPlayer()
        {
            if (currentPlayer == 1)
                return player1;
            else
                return player2;
        }

        /*
            This method check bound of matrix of fields
            So we dont acces below 0 or above 4 (5x5 -> 0..4 indexes)
        */
        public bool checkBound(int i)
        {
            if (i < 0)
                return false;
            else if (i > 4)
                return false;
            else return true;
        }

        /*
            This method check if there is figure on the field
            x - coordinate x of field we want to check
            y - coordinate y of field we want to check
        */
        public bool figureOnField(int x, int y)
        {
            if (players[0, 0] == x && players[0, 1] == y)
                return true;

            if (players[0, 2] == x && players[0, 3] == y)
                return true;

            if (players[1, 0] == x && players[1, 1] == y)
                return true;

            if (players[1, 2] == x && players[1, 3] == y)
                return true;

            return false;
        }

        /*
          This method enables all fields
          Never used, just in case we need it
        */
        public void enableAllButtons()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    buttons[i, j].IsEnabled = true;
                }
            }
        }

        /*
            This method disables all fields
        */
        public void disableAllButtons()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    buttons[i, j].IsEnabled = false;
                }
            }
        }

        /*
            This method enables fields where player can build
            x - coordinate x of field where he moved the figure
            y - coordinate y of field where he moved the figure
        */
        public void enablePossibleBuildButtons(int x, int y)
        {
            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x + 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x + 1, y) && turrets[x + 1, y] == false)
                {
                    buttons[x + 1, y].IsEnabled = true;
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x + 1, y + 1) && turrets[x + 1, y + 1] == false)
                {
                    buttons[x + 1, y + 1].IsEnabled = true;
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x + 1, y - 1) && turrets[x + 1, y - 1] == false)
                {
                    buttons[x + 1, y - 1].IsEnabled = true;
                }
            }

            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x - 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x - 1, y) && turrets[x - 1, y] == false)
                {
                    buttons[x - 1, y].IsEnabled = true;
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x - 1, y + 1) && turrets[x - 1, y + 1] == false)
                {
                    buttons[x - 1, y + 1].IsEnabled = true;
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x - 1, y - 1) && turrets[x - 1, y - 1] == false)
                {
                    buttons[x - 1, y - 1].IsEnabled = true;
                }
            }

            //CHECK IF WE HAVE COLUMN RIGHT FROM US
            if (checkBound(y + 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y + 1) && turrets[x, y + 1] == false)
                {
                    buttons[x, y + 1].IsEnabled = true;
                }
            }

            //CHECK IF WE HAVE COLUMN LEFT FROM US
            if (checkBound(y - 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y - 1) && turrets[x, y - 1] == false)
                {
                    buttons[x, y - 1].IsEnabled = true;
                }
            }
        }

        /*
            This return nodes with fields where player can build
            x - coordinate x of field where he moved the figure
            y - coordinate y of field where he moved the figure
        */
        public List<Node> getPossibleBuildButtons(int x, int y)
        {
            List<Node> children = new List<Node>();
            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x + 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x + 1, y) && turrets[x + 1, y] == false)
                {
                    children.Add(new Node(x + 1, y, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x + 1, y + 1) && turrets[x + 1, y + 1] == false)
                {
                    children.Add(new Node(x + 1, y + 1, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x + 1, y - 1) && turrets[x + 1, y - 1] == false)
                {
                    children.Add(new Node(x + 1, y - 1, true, null, 0, 0));
                }
            }

            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x - 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x - 1, y) && turrets[x - 1, y] == false)
                {
                    children.Add(new Node(x - 1, y, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x - 1, y + 1) && turrets[x - 1, y + 1] == false)
                {
                    children.Add(new Node(x - 1, y + 1, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x - 1, y - 1) && turrets[x - 1, y - 1] == false)
                {
                    children.Add(new Node(x - 1, y - 1, true, null, 0, 0));
                }
            }

            //CHECK IF WE HAVE COLUMN RIGHT FROM US
            if (checkBound(y + 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y + 1) && turrets[x, y + 1] == false)
                {
                    children.Add(new Node(x, y + 1, true, null, 0, 0));
                }
            }

            //CHECK IF WE HAVE COLUMN LEFT FROM US
            if (checkBound(y - 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y - 1) && turrets[x, y - 1] == false)
                {
                    children.Add(new Node(x, y - 1, true, null, 0, 0));
                }
            }
            return children;
        }

        /*
            This method enables fields where player can move
            x - coordinate x of field where he moved the figure
            y - coordinate y of field where he moved the figure
        */
        public void enablePossibleMoveButtons(int x, int y)
        {
            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x + 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x + 1, y) && ((fields[x + 1, y] - 1) <= fields[x, y]) && turrets[x + 1, y] == false)
                {
                    buttons[x + 1, y].IsEnabled = true;
                    possibleMoves++;
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x + 1, y + 1) && ((fields[x + 1, y + 1] - 1) <= fields[x, y]) && turrets[x + 1, y + 1] == false)
                {
                    buttons[x + 1, y + 1].IsEnabled = true;
                    possibleMoves++;
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x + 1, y - 1) && ((fields[x + 1, y - 1] - 1) <= fields[x, y]) && turrets[x + 1, y - 1] == false)
                {
                    buttons[x + 1, y - 1].IsEnabled = true;
                    possibleMoves++;
                }
            }

            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x - 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x - 1, y) && ((fields[x - 1, y] - 1) <= fields[x, y]) && turrets[x - 1, y] == false)
                {
                    buttons[x - 1, y].IsEnabled = true;
                    possibleMoves++;
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x - 1, y + 1) && ((fields[x - 1, y + 1] - 1) <= fields[x, y]) && turrets[x - 1, y + 1] == false)
                {
                    buttons[x - 1, y + 1].IsEnabled = true;
                    possibleMoves++;
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x - 1, y - 1) && ((fields[x - 1, y - 1] - 1) <= fields[x, y]) && turrets[x - 1, y - 1] == false)
                {
                    buttons[x - 1, y - 1].IsEnabled = true;
                    possibleMoves++;
                }
            }

            //CHECK IF WE HAVE COLUMN RIGHT FROM US
            if (checkBound(y + 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y + 1) && ((fields[x, y + 1] - 1) <= fields[x, y]) && turrets[x, y + 1] == false)
                {
                    buttons[x, y + 1].IsEnabled = true;
                    possibleMoves++;
                }
            }

            //CHECK IF WE HAVE COLUMN LEFT FROM US
            if (checkBound(y - 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y - 1) && ((fields[x, y - 1] - 1) <= fields[x, y]) && turrets[x, y - 1] == false)
                {
                    buttons[x, y - 1].IsEnabled = true;
                    possibleMoves++;
                }
            }
        }

        /*
            This return nodes with fields where player can move
            x - coordinate x of field where he moved the figure
            y - coordinate y of field where he moved the figure
        */
        public List<Node> getPossibleMoveButtons(int x, int y)
        {
            List<Node> children = new List<Node>();
            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x + 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x + 1, y) && ((fields[x + 1, y] - 1) <= fields[x, y]) && turrets[x + 1, y] == false)
                {
                    children.Add(new Node(x + 1, y, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x + 1, y + 1) && ((fields[x + 1, y + 1] - 1) <= fields[x, y]) && turrets[x + 1, y + 1] == false)
                {
                    children.Add(new Node(x + 1, y + 1, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x + 1, y - 1) && ((fields[x + 1, y - 1] - 1) <= fields[x, y]) && turrets[x + 1, y - 1] == false)
                {
                    children.Add(new Node(x + 1, y - 1, true, null, 0, 0));
                }
            }

            //CHECK IF WE HAVE ROW BELOW US
            if (checkBound(x - 1))
            {
                //CHECK IF WE FIGURE ON SAME COLUMN
                if (!figureOnField(x - 1, y) && ((fields[x - 1, y] - 1) <= fields[x, y]) && turrets[x - 1, y] == false)
                {
                    children.Add(new Node(x - 1, y, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN RIGHT AND NO FIGURE ON IT
                if (checkBound(y + 1) && !figureOnField(x - 1, y + 1) && ((fields[x - 1, y + 1] - 1) <= fields[x, y]) && turrets[x - 1, y + 1] == false)
                {
                    children.Add(new Node(x - 1, y + 1, true, null, 0, 0));
                }

                //CHECK IF WE HAVE COLUMN LEFT AND NO FIGURE ON IT
                if (checkBound(y - 1) && !figureOnField(x - 1, y - 1) && ((fields[x - 1, y - 1] - 1) <= fields[x, y]) && turrets[x - 1, y - 1] == false)
                {
                    children.Add(new Node(x - 1, y - 1, true, null, 0, 0));
                }
            }

            //CHECK IF WE HAVE COLUMN RIGHT FROM US
            if (checkBound(y + 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y + 1) && ((fields[x, y + 1] - 1) <= fields[x, y]) && turrets[x, y + 1] == false)
                {
                    children.Add(new Node(x, y + 1, true, null, 0, 0));
                }
            }

            //CHECK IF WE HAVE COLUMN LEFT FROM US
            if (checkBound(y - 1))
            {
                //CHECK IF WE FIGURE ON SAME ROW
                if (!figureOnField(x, y - 1) && ((fields[x, y - 1] - 1) <= fields[x, y]) && turrets[x, y - 1] == false)
                {
                    children.Add(new Node(x, y - 1, true, null, 0, 0)); ;
                }
            }
            return children;
        }

        /*
            This method changes the next turn player
            From player1 to player2 and vice versa
        */
        public void changeTurnPlayer()
        {
            switch(currentPlayer)
            {
                case 1:
                    {
                        playerOnTurn.Text = "Player 2";
                        playerOnTurn.Foreground = Brushes.Blue;
                        currentPlayer = 2;
                        break;
                    }
                case 2:
                    {
                        playerOnTurn.Text = "Player 1";
                        playerOnTurn.Foreground = Brushes.Red;
                        currentPlayer = 1;
                        break;
                    }
            }
        }

        /*
            This method changes the turn mode
            From moving to building and vice versa
        */
        public void changeTurnMode()
        {
            switch (movingMode)
            {
                case true:
                    {
                        turnMode.Text = "Building";
                        movingMode = false;
                        buildingMode = true;
                        break;
                    }
                case false:
                    {
                        turnMode.Text = "Moving";
                        movingMode = true;
                        buildingMode = false;
                        break;
                    }
            }
        }

        /*
            This method changes mode of turn from setting figures to moving
            Only called once, after figures from both players setted
        */
        public void changeTurnToMoving()
        {
            turnMode.Text = "Moving";
            movingMode = true;
            settingFigureMode = false;
            buildingMode = false;
        }

        /*
            This method set current player for winner and end the game.
        */
        public void setWinnerCurrent()
        {
            gameEnded = true;
            winnerInfo.Text = "WINNER:";
            if (currentPlayer == 1)
            {
                winner.Text = "Player 1";
                winner.Foreground = Brushes.Red;
            }
            else
            {
                winner.Text = "Player 2";
                winner.Foreground = Brushes.Blue;
            }
        }

        /*
            This method set opponent for winner and end the game.
        */
        public void setWinnerOpponent()
        {
            gameEnded = true;
            winnerInfo.Text = "WINNER:";
            if (currentPlayer == 1)
            {
                winner.Text = "Player 2";
                winner.Foreground = Brushes.Blue;
            }
            else
            {
                winner.Text = "Player 1";
                winner.Foreground = Brushes.Red;
            }
        }

        /*
            This method simulates setting turn, which is called only at start of the game.
            1. Getting current player figures
            2. Checking which figure to set
            3. Set the figure
        */
        public void playSettingTurn(int i, int j)
        {
            UIElement figure1 = null;
            UIElement figure2 = null;

            switch (currentPlayer)
            {
                case 1:
                    {
                        figure1 = Player1Figure1;
                        figure2 = Player1Figure2;
                        break;
                    }
                case 2:
                    {
                        figure1 = Player2Figure1;
                        figure2 = Player2Figure2;
                        break;
                    }
            }

            //CHECK IF FIGURE 1 IS SET
            if (players[currentPlayer - 1, 0] == -1)
            {
                setFigure(i, j, figure1, 0, 1);
            }
            //OTHERWISE IT MUST LEFT FIGURE 2 TO BE SET
            else
            {
                setFigure(i, j, figure2, 2, 3);
                if (readyPlayer1 == false)
                {
                    readyPlayer1 = true;
                    player1.Ready = true;
                    changeTurnPlayer();
                }
                else if (readyPlayer2 == false)
                {
                    readyPlayer2 = true;
                    player2.Ready = true;
                    selectingFigure = true;
                    disableAllButtons();
                    changeTurnToMoving();
                    changeTurnPlayer();
                    enablePlayerFigures();
                }
                else
                    changeTurnPlayer();                
            }
        }

        /*
            This method simulates moving turn (include the setting figures at start)
            1. Check if player has setted figures
                1.1 Check if he setted first figure
                1.2 Check if he setted second figure
            2. Check the field he's moving to
                2.1 If has less than 3, just move the figure
                2.2 If has 3, move figure and wins the game
            3. If game is not over, move to building turn
        */
        public void playMovingTurn(int i, int j)
        {
            if (endOfGame())
                return;

            UIElement figure1 = null;
            UIElement figure2 = null;

            switch (currentPlayer)
            {
                case 1:
                    {
                        figure1 = Player1Figure1;
                        figure2 = Player1Figure2;
                        break;
                    }
                case 2:
                    {
                        figure1 = Player2Figure1;
                        figure2 = Player2Figure2;
                        break;
                    }
            }

            //CHECK IF IT HAS LESS THAN 3 PLOTS
            if (fields[i, j] < 3)
            {
                moveFigure(Grid.GetRow(currentFigure), Grid.GetColumn(currentFigure), i, j, currentFigure, currentFigureX, currentFigureY);
                disableAllButtons();
                enablePossibleBuildButtons(i, j);
                changeTurnMode();
            }
            //OTHERWISE PLAYER WIN THE GAME
            else
            {
                moveFigure(Grid.GetRow(currentFigure), Grid.GetColumn(currentFigure), i, j, currentFigure, currentFigureX, currentFigureY);
                disableAllButtons();
                setWinnerCurrent();
            }
        }

        /*
            This method simulates the building turn. 
            1. If we build on field that has less than 3 plots, we increment it.
            2. If we build on field that has 3 plots, we build ellipse on it and disable that field
            3. After building we check if other player can move somewhere which means we check for the end of the game
             
            i - coordinate x
            j - coordinate y
        */
        public void playBuildingTurn(int i, int j)
        {
            if (endOfGame())
                return;

            if (fields[i, j] < 3)
            {
                fields[i, j]++;
                String text = texts[i, j].Text;
                texts[i, j].Text = (Int32.Parse(texts[i, j].Text) + 1).ToString();
            }
            else
            {
                ellipses[i, j].Opacity = 1.0;
                turrets[i, j] = true;
                buttons[i, j].IsEnabled = false;
            }
            selectingFigure = true;
            changeTurnPlayer();
            changeTurnMode();
            disableAllButtons();
            if (endOfGame())
                setWinnerOpponent();
            else
            {
                if (getCurrentPlayer().GetType() != typeof(Human))
                    disableAllButtons();
                else
                    enablePlayerFigures();
            }
                
        }

        /*
            This method updates currentFigure with the figure that Player have selected
            x - coordinate x
            y - coordinate y
        */
        public void selectFigure(int x, int y)
        {
            switch (currentPlayer)
            {
                case 1:
                    {
                        if (players[currentPlayer - 1, 0] == x && players[currentPlayer - 1, 1] == y)
                        {
                            currentFigure = Player1Figure1;
                            currentFigureX = 0;
                            currentFigureY = 1;
                        }
                        else
                        {
                            currentFigure = Player1Figure2;
                            currentFigureX = 2;
                            currentFigureY = 3;
                        }
                        break;
                    }
                case 2:
                    {
                        if (players[currentPlayer - 1, 0] == x && players[currentPlayer - 1, 1] == y)
                        {
                            currentFigure = Player2Figure1;
                            currentFigureX = 0;
                            currentFigureY = 1;
                        }
                        else
                        {
                            currentFigure = Player2Figure2;
                            currentFigureX = 2;
                            currentFigureY = 3;
                        }
                        break;
                    }
            }
        }

        /*
            This method set figure on field, only called on the start of the game.
            Initializing the fields.
        */
        public void setFigure(int i, int j, UIElement element, int x, int y)
        {
            mainGrid.Children.Remove(element);
            grids[i, j].Children.Add(element);
            Grid.SetRow(element, i);
            Grid.SetColumn(element, j);
            buttons[i, j].IsEnabled = false;
            element.IsEnabled = true;

            //X - x axis, Y - y axis
            players[currentPlayer - 1, x] = i;
            players[currentPlayer - 1, y] = j;
        }

        /*
           This method moves figure from field to field.
           i - coordinate x from where we move
           j - coordinate y from where we move
           m - coordinate x to where we move
           n - coordinate y to where we move
           x - index of coordinate x of figure in players array
           y - index of coordinate y of figure in players array
       */
        public void moveFigure(int i, int j, int m, int n, UIElement element, int x, int y)
        {
            grids[i, j].Children.Remove(element);
            Grid.SetRow(element, m);
            Grid.SetColumn(element, n);
            grids[m, n].Children.Add(element);

            players[currentPlayer - 1, x] = m;
            players[currentPlayer - 1, y] = n;

        }

        /*
            This method enables buttons that have player's figure on them
        */
        public void enablePlayerFigures()
        {
            switch(currentPlayer)
            {
                case 1:
                    {
                        buttons[players[0, 0], players[0, 1]].IsEnabled = true;
                        Player1Figure1.IsEnabled = true;
                        buttons[players[0, 2], players[0,3]].IsEnabled = true;
                        Player1Figure2.IsEnabled = true;
                        break;
                    }
                case 2:
                    {
                        buttons[players[1, 0], players[1, 1]].IsEnabled = true;
                        Player2Figure1.IsEnabled = true;
                        buttons[players[1, 2], players[1, 3]].IsEnabled = true;
                        Player2Figure2.IsEnabled = true;
                        break;
                    }
            }
        }

        /*
            This method check if its the end of the game
            Checking is done by counting possible move fields for both figures
        */
        public bool endOfGame()
        {
            if (currentPlayer == 1)
            {
                possibleMoves = 0;
                enablePossibleMoveButtons(players[0, 0], players[0, 1]);
                enablePossibleMoveButtons(players[0, 2], players[0, 3]);
                disableAllButtons();
                if (possibleMoves == 0)
                    return true;
                else
                    return false;
            }
            else
            {
                possibleMoves = 0;
                enablePossibleMoveButtons(players[1, 0], players[1, 1]);
                enablePossibleMoveButtons(players[1, 2], players[1, 3]);
                disableAllButtons();
                if (possibleMoves == 0)
                    return true;
                else
                    return false;
            }
        }

        /*
            Mouse click on button occurs this method. 
            Multiple function this method has, depends on mode of turn
        */
        public void buttonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int i = Grid.GetRow(button);
            int j = Grid.GetColumn(button);

            if (selectingFigure == true)
            {
                disableAllButtons();
                enablePossibleMoveButtons(i, j);
                selectFigure(i, j);
                selectingFigure = false;
            }
            else
            {
                if(settingFigureMode == true)
                {
                    playSettingTurn(i, j);
                }
                else if(movingMode == true)
                {
                    playMovingTurn(i, j);
                }
                else
                {
                    playBuildingTurn(i, j);
                }
            }
        }   
        
        /*
            Event on button to play one turn.
        */
        public void playOne(object sender, RoutedEventArgs e)
        {
            if(settingFigureMode == true)
                getCurrentPlayer().setMyFigure();
            else
                getCurrentPlayer().playTurn();
        }

        /*
            Event on button to play all turns.
        */
        public void playAll(object sender, RoutedEventArgs e)
        {
            if (settingFigureMode == true)
            {
                for (int i = 0; i < 4; i++)
                    getCurrentPlayer().setMyFigure();
            }
            else
            { 
                while(!gameEnded)
                    getCurrentPlayer().playTurn();
            }
        }
    }
}