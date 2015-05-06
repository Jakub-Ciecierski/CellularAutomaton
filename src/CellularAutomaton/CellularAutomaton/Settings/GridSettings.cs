using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Settings
{
    public class GridSettings
    {

        private static Color aliveColor = Color.FromArgb(255, 255, 255);

        public static Color AliveColor
        {
            get { return aliveColor; }
            private set { aliveColor = value; }
        }

        private static Color deadColor = Color.FromArgb(0, 0, 0);

        public static Color DeadColor
        {
            get { return deadColor; }
            private set { deadColor = value; }
        }

        public static Color GetStateColor(int state)
        {
            if (state == CellStates.DEAD)
                return DeadColor;
            else if (state == CellStates.ALIVE)
                return AliveColor;
            throw new NotImplementedException("Unknow state");
        }
    }
}
