using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    public class ExtendedMooreNeighborhood : Neighborhood
    {
        public ExtendedMooreNeighborhood(int localCell, int[] neighbors)
            : base(localCell, neighbors)
        {
        }

    }
}
