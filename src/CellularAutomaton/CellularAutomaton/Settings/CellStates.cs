using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Settings
{
    public class CellStates
    {
        public const int DEAD = 0;
        public const int ALIVE = 1;

        public static int GetOpositeState(int state)
        {
            if (state == DEAD)
                return ALIVE;
            else
                return DEAD;
        }
    }
}
