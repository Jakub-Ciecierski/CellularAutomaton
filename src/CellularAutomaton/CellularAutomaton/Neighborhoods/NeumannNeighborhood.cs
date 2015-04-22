using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    /// <summary>
    ///     Represents a 4-point neighborhood
    /// </summary>
    public class NeumannNeighborhood : Neighborhood
    {
        /// <summary>
        ///     The count of neighbors in this
        ///     neighborhood
        /// </summary>
        public const int NEIGHBOR_COUNT = 4;

        public NeumannNeighborhood(int localCell, int[] neighbors) 
            : base(localCell, neighbors)
        {
        }

        public override int NeighborCount(int state)
        {
            int count = 0;
            for (int i = 0; i < NEIGHBOR_COUNT; i++)
                if (neighbors[i] == state)
                    count++;
            return count;
        }
    }
}
