using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace jm150635d
{
    public abstract class Player
    {
        public static readonly double MIN_VALUE = -10000;
        public static readonly double MAX_VALUE = 10000;

        protected Node root;
        public Node Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }
        
        protected bool ready;
        public bool Ready
        {
            get
            {
                return ready;
            }
            set
            {
                ready = value;
            }
        }

        protected GamePage gamePage;
        public GamePage GamePage
        {
            get
            {
                return gamePage;
            }
            set
            {
                gamePage = value;
            }
        }

        protected bool isMaxPlayer;
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

        protected int treeDepth;
        public int TreeDepth
        {
            get
            {
                return treeDepth;
            }
            set
            {
                treeDepth = value;
            }
        }

        protected UIElement figure1;
        public UIElement Figure1
        {
            get
            {
                return figure1;
            }
            set
            {
                figure1 = value;
            }
        }

        protected UIElement figure2;
        public UIElement Figure2
        {
            get
            {
                return figure2;
            }
            set
            {
                figure2 = value;
            }
        }
        
        protected Player opponent;
        public Player Opponent
        {
            get
            {
                return opponent;
            }
            set
            {
                opponent = value;
            }
        }

        public abstract double calculateMoveFunction(int i, int j);
        public abstract double calculateBuildFunction(int i, int j);
        public abstract void playTurn();


        public void setMyFigure()
        {
            Random random = new Random();
            int try1 = random.Next(0, 5);
            int try2 = random.Next(0, 5);
            while (gamePage.figureOnField(try1, try2))
            {
                try1 = random.Next(0, 5);
                try2 = random.Next(0, 5);
            }

            gamePage.playSettingTurn(try1, try2);
        }
    }
}
