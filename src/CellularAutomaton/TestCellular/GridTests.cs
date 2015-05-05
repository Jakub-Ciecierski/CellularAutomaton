using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CellularAutomaton;
using CellularAutomaton.Neighborhoods;

namespace TestCellular
{
    [TestClass]
    public class GridTests
    {
        [TestMethod]
        public void GetMooreNeighborhood_Test()
        {
            CellularAutomaton.CellularGrid grid = new CellularAutomaton.CellularGrid(10, 10, 0);
            grid.SetState(9, 9, 1);
            grid.SetState(0, 9, 1);
            grid.SetState(1, 9, 1);
            grid.SetState(9, 0, 1);
            grid.SetState(9, 1, 1);
            grid.Show();

            MooreNeighborhood actualNb = (MooreNeighborhood)grid.GetNeighborhood(0, 0, NeighborhoodTypes.Moore);

            int[] neightboors = {1,1,1,1,0,1,0,0};
            MooreNeighborhood expectedNb = new MooreNeighborhood(0, neightboors);

            Assert.AreEqual(expectedNb, actualNb);
        }

        [TestMethod]
        public void GetExtendedMooreNeighborhood_Test()
        {
            CellularAutomaton.CellularGrid grid = new CellularAutomaton.CellularGrid(10, 10, 0);
            grid.SetState(9, 9, 1);
            grid.SetState(8, 8, 1);
            grid.SetState(0, 9, 1);
            grid.SetState(1, 9, 1);
            grid.SetState(8, 1, 1);
            grid.SetState(9, 0, 1);
            grid.SetState(9, 1, 1);
            grid.Show();

            ExtendedMooreNeighborhood actualNb = (ExtendedMooreNeighborhood)grid.GetNeighborhood(0, 0, NeighborhoodTypes.ExtendedMoore);

            int[] neightboors = {   1, 0, 0, 1, 0, 
                                    0, 1, 1, 1, 0, 
                                    0, 1, 0, 0, 
                                    0, 1, 0, 0, 0, 
                                    0, 0, 0, 0, 0, };
            ExtendedMooreNeighborhood expectedNb = new ExtendedMooreNeighborhood(0, neightboors);

            Assert.AreEqual(expectedNb, actualNb);
        }
    }
}
