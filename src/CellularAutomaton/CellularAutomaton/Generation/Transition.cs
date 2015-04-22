using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Generation
{
    public class Transition
    {
        /// <summary>
        ///     The delegate which encapsulates transition functions
        /// </summary>
        /// <param name="nb">
        ///     Neighborhood to check for transition.
        /// </param>
        /// <returns>
        ///     True if neighborhood is eligible for transition
        /// </returns>
        public delegate bool TransitionDelegate(Neighborhood nb);

        /// <summary>
        ///     The new state to which we should transition
        /// </summary>
        private int newState;

        /// <summary>
        ///     Handle to transition function
        /// </summary>
        private TransitionDelegate transitionFunction;

        public Transition()
        {

        }

        public Transition(int newState, TransitionDelegate transitionFunction) 
        {
            this.newState = newState;
            this.transitionFunction = transitionFunction;
        }

        /// <summary>
        ///     Applies the transition to given neighborhood
        /// </summary>
        /// <param name="nb">
        ///     Neighbrohood to compute against
        /// </param>
        /// <returns>
        ///     New state or -1 if neighborhood was not eligible
        /// </returns>
        public int Apply(Neighborhood nb)
        {
            if (transitionFunction(nb))
                return newState;
            else
                return -1;
        }
    }
}
