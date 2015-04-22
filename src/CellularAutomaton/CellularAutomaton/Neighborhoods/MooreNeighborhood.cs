using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    public class MooreNeighborhood : Neighborhood
    {
        public MooreNeighborhood(int localCell, int[] neighbors)
            : base(localCell, neighbors)
        {
        }

        public override int NeighborCount(int state)
        {
            return state;
        }
    }
}
