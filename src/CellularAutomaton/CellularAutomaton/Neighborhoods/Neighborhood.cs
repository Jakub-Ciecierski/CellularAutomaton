using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    [Serializable]
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
        public int[] neighbors;

        public Neighborhood(int localCell, int[] neighbors)
        {
            this.localCell = localCell;
            this.neighbors = neighbors;
        }

        public int NeighborCount(int state)
        {
            int count = 0;
            for (int i = 0; i < neighbors.Count(); i++)
            {
                if (neighbors[i] == state)
                    count++;
            }
            return count;
        }

        public override bool Equals(object obj)
        {
            Neighborhood nb = obj as Neighborhood;
            return (neighbors.SequenceEqual(nb.neighbors) && LocalCell == nb.LocalCell);
        }
    }
}
