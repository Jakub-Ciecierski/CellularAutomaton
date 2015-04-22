using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    public abstract class Neighborhood
    {
        /// <summary>
        ///     The cell which neighborhood we consider
        /// </summary>
        protected int localCell;
        public int LocalCell 
        { 
            get { return localCell; } 
            private set {localCell = value; } 
        }
        /// <summary>
        ///     The actual neighbors in the cell's vicinity
        /// </summary>
        protected int[] neighbors;

        public Neighborhood(int localCell, int[] neighbors)
        {
            this.localCell = localCell;
            this.neighbors = neighbors;
        }

        abstract public int NeighborCount(int state);
    }
}
