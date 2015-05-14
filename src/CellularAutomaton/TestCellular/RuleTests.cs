using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CellularAutomaton;
using CellularAutomaton.Neighborhoods;
using CellularAutomaton.Generation;

namespace TestCellular
{
    [TestClass]
    public class RuleTests
    {
        [TestMethod]
        public void Transition_NewTransition_Accepted()
        {
            /*
            int[] n = {1,2};
            MooreNeighborhood nb = new MooreNeighborhood(0,n);

            int expectedNewState = 1;
            Transition transition = new Transition(expectedNewState, x => { return true; });

            int actualNewState = transition.Apply(nb);

            Assert.AreEqual(expectedNewState, actualNewState);
             * */
        }
    }
}
