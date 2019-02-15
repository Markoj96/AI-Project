using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace jm150635d
{
    class Easy : Player
    {
        public Easy(bool isMax, UIElement figure1, UIElement figure2, int treeDepth)
        {
            this.isMaxPlayer = isMax;
            this.figure1 = figure1;
            this.figure2 = figure2;
            this.treeDepth = treeDepth;
        }

        public double getDistance(int i, int j)
        {
            double myDistance1 = Math.Sqrt(Math.Pow(Grid.GetRow(Figure1) - i, 2) + Math.Pow(Grid.GetColumn(Figure1) - j, 2));
            double myDistance2 = Math.Sqrt(Math.Pow(Grid.GetRow(Figure2) - i, 2) + Math.Pow(Grid.GetColumn(Figure2) - j, 2));

            double oppDistance1 = Math.Sqrt(Math.Pow(Grid.GetRow(Opponent.Figure1) - i, 2) + Math.Pow(Grid.GetColumn(Opponent.Figure1) - j, 2));
            double oppDistance2 = Math.Sqrt(Math.Pow(Grid.GetRow(Opponent.Figure2) - i, 2) + Math.Pow(Grid.GetColumn(Opponent.Figure2) - j, 2));

            return ((myDistance1 + myDistance2) - (oppDistance1 + oppDistance2));
        } 

        public override double calculateMoveFunction(int i, int j)
        {

            return (gamePage.fields[i, j] + (gamePage.fields[i, j] * getDistance(i, j)));
        }

        public override double calculateBuildFunction(int i, int j)
        {

            return (gamePage.fields[i, j] + ((gamePage.fields[i, j] + 1) * getDistance(i, j)));
        }
        
        public override void playTurn()
        {
            root = new Node(0, 0, isMaxPlayer, Figure1, 0, 0);
            constructMoveTree(new Node(Grid.GetRow(Figure1), Grid.GetColumn(Figure1), isMaxPlayer, Figure1, 0, 0), new Node(Grid.GetRow(Figure2), Grid.GetColumn(Figure2), isMaxPlayer, Figure2, 0, 0), treeDepth);
            Node best = root.getBestChildren();

            int i = best.Row;
            int j = best.Column;

            int x = Grid.GetRow(best.Figure);
            int y = Grid.GetColumn(best.Figure);

            gamePage.selectFigure(x, y);
            gamePage.playMovingTurn(i, j);
            System.Threading.Thread.Sleep(1000);

            root = new Node(0, 0, isMaxPlayer, Figure1, 0, 0);
            constructBuildTree(new Node(i, j, isMaxPlayer, best.Figure, 0, 0), treeDepth);
            best = root.getBestChildren();

            i = best.Row;
            j = best.Column;

            x = Grid.GetRow(best.Figure);
            y = Grid.GetColumn(best.Figure);

            gamePage.selectFigure(x, y);
            gamePage.playBuildingTurn(i, j);
            System.Threading.Thread.Sleep(1000);
        }

        private double minimaxMove(Node node, int treeDepth, bool isMax)
        {
            if(treeDepth == 0)
            {

                return calculateMoveFunction(node.Row, node.Column); // CALCULATE FUNCTION OF CURRENT NODE
            }
            if (isMax)
            {
                double bestValue = Player.MIN_VALUE;
                List<Node> children = createMoveChildren(node);
                for(int i = 0; i < children.Count; i++)
                {
                    Node next = children[i];
                    double value = minimaxMove(next, treeDepth - 1, false);
                    bestValue = Math.Max(bestValue, value);
                }
                node.Value = bestValue;
                return bestValue;
            }
            else
            {
                double bestValue = Player.MAX_VALUE;
                List<Node> children = createMoveChildren(node);
                for (int i = 0; i < children.Count; i++)
                {
                    Node next = children[i];
                    double value = minimaxMove(next, treeDepth - 1, true);
                    bestValue = Math.Min(bestValue, value);
                }
                node.Value = bestValue;
                return bestValue;
            }
        }

        public List<Node> createMoveChildren(Node root)
        {
            List<Node> children = gamePage.getPossibleMoveButtons(Grid.GetRow(root.Figure), Grid.GetColumn(root.Figure));

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Figure = root.Figure;
                children[i].FigureX = root.FigureX;
                children[i].FigureY = root.FigureY;
                children[i].IsMaxPlayer = !root.IsMaxPlayer;
            }

            return children;
        }

        public void constructMoveTree(Node root1, Node root2, int height)
        {
            List<Node> children1 = createMoveChildren(root1);
            List<Node> children2 = createMoveChildren(root2);
            List<Node> children = new List<Node>();

            for (int i = 0; i < children1.Count; i++)
                children.Add(children1[i]);

            for (int i = 0; i < children2.Count; i++)
                children.Add(children2[i]);

            for (int i = 0; i < children.Count; i++)
            {
                Node next = children[i];
                double value = minimaxMove(next, treeDepth - 1, !next.IsMaxPlayer);
                next.Value = value;
                root.Children = children[i];
            }
        }

        private double minimaxBuild(Node node, int treeDepth, bool isMax)
        {
            if (treeDepth == 0)
            {
                return calculateBuildFunction(node.Row, node.Column); // CALCULATE FUNCTION OF CURRENT NODE
            }
            if (isMax)
            {
                double bestValue = Player.MIN_VALUE;
                List<Node> children = createBuildChildren(node);
                for (int i = 0; i < children.Count; i++)
                {
                    Node next = children[i];
                    double value = minimaxBuild(next, treeDepth - 1, false);
                    bestValue = Math.Max(bestValue, value);
                }
                node.Value = bestValue;
                return bestValue;
            }
            else
            {
                double bestValue = Player.MAX_VALUE;
                List<Node> children = createBuildChildren(node);
                for (int i = 0; i < children.Count; i++)
                {
                    Node next = children[i];
                    double value = minimaxBuild(next, treeDepth - 1, true);
                    bestValue = Math.Min(bestValue, value);
                }
                node.Value = bestValue;
                return bestValue;
            }
        }

        public List<Node> createBuildChildren(Node root)
        {
            List<Node> children = gamePage.getPossibleBuildButtons(Grid.GetRow(root.Figure), Grid.GetColumn(root.Figure));

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Figure = root.Figure;
                children[i].FigureX = root.FigureX;
                children[i].FigureY = root.FigureY;
                children[i].IsMaxPlayer = !root.IsMaxPlayer;
            }

            return children;
        }

        public void constructBuildTree(Node root, int height)
        {
            List<Node> children = createBuildChildren(root);
            
            for (int i = 0; i < children.Count; i++)
            {
                Node next = children[i];
                double value = minimaxBuild(next, treeDepth - 1, !next.IsMaxPlayer);
                next.Value = value;
                this.root.Children = children[i];
            }
        }      
    }
}
