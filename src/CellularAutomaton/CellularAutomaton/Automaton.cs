﻿using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton
{
    /// <summary>
    ///     This class describes the cellular automaton.
    ///     Holds information about the grid of cells, 
    ///     current rule.
    /// </summary>
    public class Automaton
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     The grid of cells
        /// </summary>
        private Grid grid;

        /// <summary>
        ///     Current rule of the automaton
        /// </summary>
        private Rule currentRule;

        public Rule CurrentRule
        {
            get { return currentRule; }
            set { currentRule = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        /// <summary>
        ///     The constructor
        /// </summary>
        public Automaton(Grid grid)
        {
            this.grid = grid;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/


        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Computes next generation of this automaton
        /// </summary>
        public void NextGeneration()
        {
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    Neighborhood nb = grid.GetNeighborhood(i, j, CurrentRule.NeighborhoodType);
                    grid.SetState(i, j, CurrentRule.Apply(nb));
                }
            }
        }

        public void Show()
        {
            grid.Show();
        }
    }
}