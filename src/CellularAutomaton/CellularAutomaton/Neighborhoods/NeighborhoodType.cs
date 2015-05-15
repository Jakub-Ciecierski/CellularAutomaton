using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Neighborhoods
{
    [Serializable]
    public enum NeighborhoodType
    {
        Neumann,
        Moore,
        ExtendedMoore,
        None
    }
}
