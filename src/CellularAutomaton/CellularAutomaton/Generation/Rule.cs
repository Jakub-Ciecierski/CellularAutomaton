using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Generation
{
    /// <summary>
    ///     Rule is a set of Transitions, which is used to
    ///     compute next generation of the Automaton
    /// </summary>
    public class Rule
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     The list of transtitions defined
        ///     in this rule
        /// </summary>
        private List<Transition> transitions = new List<Transition>();

        /// <summary>
        ///     The dafualt transition is used when no transition
        ///     is found in the list for the given neighborhood.
        /// </summary>
        private Transition defaultTransition;

        /// <summary>
        ///     The type of the neighborhood which this rule is 
        ///     defined over.
        /// </summary>
        private NeighborhoodTypes neighborhoodType;

        public NeighborhoodTypes NeighborhoodType
        {
            get { return neighborhoodType; }
            private set { neighborhoodType = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Rule(NeighborhoodTypes neighborhoodType)
        {
            NeighborhoodType = neighborhoodType;
            defaultTransition = new Transition();
        }

        /// <summary>
        ///     Creates rule for given neighborhood and
        ///     with default transition
        ///     
        /// </summary>
        /// <param name="neighborhoodType"></param>
        /// <param name="defaultTransition"></param>
        public Rule(NeighborhoodTypes neighborhoodType, Transition defaultTransition)
        {
            NeighborhoodType = neighborhoodType;
            this.defaultTransition = defaultTransition;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/



        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Adds new transition
        /// </summary>
        /// <param name="transition">
        ///     The transition to be added
        /// </param>
        public void AddTransition(Transition transition)
        {
            transitions.Add(transition);
        }

        /// <summary>
        ///     Removes transition from the list
        /// </summary>
        /// <param name="transition">
        ///     Transition to be removed
        /// </param>
        public void RemoveTransition(Transition transition)
        {
            transitions.Remove(transition);
        }

        /// <summary>
        ///     Applies rule to given neighborhood
        /// </summary>
        /// <param name="nb">
        ///     Neighborhood to apply rule against
        /// </param>
        /// <returns>
        ///     new state
        /// </returns>
        public int Apply(Neighborhood nb)
        {
            foreach (Transition transition in transitions)
            {
                int newState;
                if((newState=transition.Apply(nb)) >= 0)
                    return newState;
            }
            return nb.LocalCell;
            // return defaultTransition.Apply(nb);
        }
    }
}
