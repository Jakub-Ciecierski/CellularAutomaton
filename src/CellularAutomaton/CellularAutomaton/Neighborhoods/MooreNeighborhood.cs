using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    /// <summary>
    ///     Neighbors are stored as follows:
    ///     
    ///     neighbors[] = {NW,N,NE,W,E,SW,S,SE}
    ///     
    ///     NW - north west, etc.
    /// </summary>
    public class MooreNeighborhood : Neighborhood
    {
        /// <summary>
        ///     The count of neighbors in this
        ///     neighborhood
        /// </summary>
        public const int NEIGHBOR_COUNT = 8;

        public MooreNeighborhood(int localCell, int[] neighbors)
            : base(localCell, neighbors)
        {
        }

    }
}
