using CellularAutomaton.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Generation
{
    [Serializable]
    public class SimpleTransition : Transition
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private int[] transitionMap;

        public int[] TransitionMap
        {
            get { return transitionMap; }
            set { transitionMap = value; }
        }

        private int newState;

        public int NewState
        {
            get { return newState; }
            set { newState = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public SimpleTransition(int[] transitionMap, int newState)
        {
            TransitionMap = transitionMap;
            NewState = newState;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
        public override int Apply(Neighborhoods.Neighborhood nb)
        {
            if (nb.LocalCell == TransitionMap[0] &&
                (nb.NeighborCount(CellStates.DEAD) == TransitionMap[1] || TransitionMap[1] == -1) &&
                nb.NeighborCount(CellStates.ALIVE) == TransitionMap[2] || TransitionMap[2] == -1)
                return NewState;
            else
                return -1;
        }
    }
}
