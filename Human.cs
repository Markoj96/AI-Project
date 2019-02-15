using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jm150635d
{
    class Human : Player
    {
        /*
            In Human case, this method wont do anything.
            We will play with mouse
        */
        public override double calculateMoveFunction(int i, int j)
        {
            return 0;
        }

        public override double calculateBuildFunction(int i, int j)
        {
            return 0;
        }

        public override void playTurn()
        {
            return;
        }
    }
}
