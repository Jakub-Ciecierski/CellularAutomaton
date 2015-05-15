using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Generation
{
    [Serializable]
    public abstract class Transition
    {

        /// <summary>
        ///     Applies the transition to given neighborhood
        /// </summary>
        /// <param name="nb">
        ///     Neighbrohood to compute against
        /// </param>
        /// <returns>
        ///     New state or -1 if neighborhood was not eligible
        /// </returns>
        public abstract int Apply(Neighborhood nb);
    }
}
