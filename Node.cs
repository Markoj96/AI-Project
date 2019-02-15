using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace jm150635d
{
    public class Node
    {
        private List<Node> children;
        public Node Children
        {
            set
            {
                this.children.Add(value);
            }
        }
        public List<Node> Childrens
        {
            get
            {
                return this.children;
            }
        }

        private bool isMaxPlayer;
        public bool IsMaxPlayer
        {
            get
            {
                return this.isMaxPlayer;
            }
            set
            {
                this.isMaxPlayer = value;
            }
        }

        private double value;
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        private int row;
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
            }
        }

        private int column;
        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }

        private UIElement figure;
        public UIElement Figure
        {
            get
            {
                return figure;
            }
            set
            {
                figure = value;
            }
        }

        private int figureX;
        public int FigureX
        {
            get
            {
                return figureX;
            }
            set
            {
                figureX = value;
            }
        }

        private int figureY;
        public int FigureY
        {
            get
            {
                return figureY;
            }
            set
            {
                figureY = value;
            }
        }

        public Node getBestChildren()
        {
            Node bestChild = null;
            double bestValue = 0;
            if(isMaxPlayer  == true)
            {
                bestValue = Player.MIN_VALUE;
                for(int i = 0; i < children.Count; i++)
                {
                    if(children[i].value > bestValue)
                    {
                        bestValue = children[i].value;
                        bestChild = children[i];
                    }
                }

                return bestChild;
            }
            else
            {
                bestValue = Player.MAX_VALUE;
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].value < bestValue)
                    {
                        bestValue = children[i].value;
                        bestChild = children[i];
                    }
                }

                return bestChild;
            }

            return null;
        }

        /*
            Same method as above, just used for hard comp
            If there is possible field with 3 plots to move, take it and win the game
        */
        public Node getBestChildren(GamePage gamePage)
        {
            Node bestChild = null;
            double bestValue = 0;
            if (isMaxPlayer == true)
            {
                bestValue = Player.MIN_VALUE;
                for (int i = 0; i < children.Count; i++)
                {
                    if(gamePage.fields[children[i].Row, children[i].Column] == 3)
                    {
                        bestChild = children[i];
                        break;
                    }
                    if (children[i].value > bestValue)
                    {
                        bestValue = children[i].value;
                        bestChild = children[i];
                    }
                }

                return bestChild;
            }
            else
            {
                bestValue = Player.MAX_VALUE;
                for (int i = 0; i < children.Count; i++)
                {
                    if (gamePage.fields[children[i].Row, children[i].Column] == 3)
                    {
                        bestChild = children[i];
                        break;
                    }
                    if (children[i].value < bestValue)
                    {
                        bestValue = children[i].value;
                        bestChild = children[i];
                    }
                }

                return bestChild;
            }

            return null;
        }

        public Node(int i, int j, bool isMax, UIElement figure, int x, int y)
        {
            isMaxPlayer = isMax;
            row = i;
            column = j;
            children = new List<Node>();
            this.figure = figure;
            figureX = x;
            figureY = y;
        }
    }
}
