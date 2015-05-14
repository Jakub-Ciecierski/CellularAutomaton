using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Generation
{
    public class DetailedTransition : Transition
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private Neighborhood neighborhood;

        public Neighborhood Neighborhood
        {
            get { return neighborhood; }
            set { neighborhood = value; }
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

        public DetailedTransition(Neighborhood neighborhood, int newState)
        {
            Neighborhood = neighborhood;
            NewState = newState;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/



        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
        public override int Apply(Neighborhood nb)
        {
            if (nb.Equals(neighborhood))
                return NewState;
            else
                return -1;
        }
    }
}
