using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    /// <summary>
    ///     Represents a 4-point neighborhood
    ///     
    ///     neighbors[] = {N,E,S,W}
    /// </summary>
    [Serializable]
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

    }
}
