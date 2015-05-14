using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
        ///     Number of thread running in parallel
        /// </summary>
        const int NUM_THREADS = 4;

        /// <summary>
        ///     The grid of cells
        /// </summary>
        private CellularGrid grid;

        public CellularGrid Grid
        {
            get { return grid; }
            private set { grid = value; }
        }
        

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
        public Automaton(CellularGrid grid)
        {
            this.grid = grid;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void nextGenerationPartial()
        {
            ConsoleManager.CaptureTime();

            int[][] flags = new int[grid.Height][];

            // prepapre tmp state array
            int[][] tmpStates = new int[grid.Height][];
            for (int i = 0; i < grid.Height; i++)
            {
                tmpStates[i] = new int[grid.Width];
                flags[i] = new int[grid.Width];
            }

            //ConsoleManager.PrintElapsedTime("ARRAY INIT");
            //ConsoleManager.CaptureTime();

            
            Thread[] threads = new Thread[NUM_THREADS];

            int partialHeight = (grid.Height / NUM_THREADS ) + 1;
            int partialWidth = (grid.Width / NUM_THREADS) + 1;

            for (int t = 0; t < NUM_THREADS; t++)
            {
                var tt = t;
                threads[tt] = new Thread(delegate()
                {
                    //Console.Write("Thread #" + tt + " From H: " + tt * partialHeight + " To: " + (tt + 1) * partialHeight + "\n");
                    for (int i = tt * partialHeight; i < (tt + 1) * partialHeight; i++)
                    {
                        for (int j = 0; j < grid.Width; j++)
                        {
                            if(i<grid.Height && j<grid.Width)
                            {
                                //ConsoleManager.CaptureTime();
                                Neighborhood nb = grid.GetNeighborhood(i, j, CurrentRule.NeighborhoodType);
                                //nbTime += ConsoleManager.GetElapsedTime();

                                //ConsoleManager.CaptureTime();
                                tmpStates[i][j] = CurrentRule.Apply(nb);
                                //applyTime += ConsoleManager.GetElapsedTime();

                                if (tmpStates[i][j] != grid.GetState(i, j))
                                    flags[i][j] = 1;
                            }
                        }
                    }
                });
                threads[t].Start();
            }

            for (int t = 0; t < NUM_THREADS; t++)
            {
                threads[t].Join();
            }

            //ConsoleManager.PrintElapsedTime("APPLY RULES");
            //ConsoleManager.CaptureTime();

            // replace new states
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (flags[i][j] == 1)
                        grid.SetState(i, j, tmpStates[i][j]);
                }
            }


            //ConsoleManager.PrintElapsedTime("DRAWING");
            //ConsoleManager.CaptureTime();
        }

        private void nextGenerationSeq()
        {
            ConsoleManager.CaptureTime();

            // check if given cell should be redrawn
            int[][] flags = new int[grid.Height][];

            // prepapre tmp state array
            int[][] tmpStates = new int[grid.Height][];
            for (int i = 0; i < grid.Height; i++)
            {
                tmpStates[i] = new int[grid.Width];
                flags[i] = new int[grid.Width];
            }

            ConsoleManager.PrintElapsedTime("ARRAY INIT");
            ConsoleManager.CaptureTime();

            long nbTime = 0;
            long applyTime = 0;

            // Compute new state
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    ConsoleManager.CaptureTime();
                    Neighborhood nb = grid.GetNeighborhood(i, j, CurrentRule.NeighborhoodType);
                    nbTime += ConsoleManager.GetElapsedTime();

                    ConsoleManager.CaptureTime();
                    tmpStates[i][j] = CurrentRule.Apply(nb);
                    applyTime += ConsoleManager.GetElapsedTime();

                    if (tmpStates[i][j] != grid.GetState(i, j))
                        flags[i][j] = 1;
                }
            }

            ConsoleManager.PrintElapsedTime("APPLY RULES" + "\n Time GetNeighborhood: " + nbTime + " Time Apply rule: " + applyTime);
            ConsoleManager.CaptureTime();

            // replace new states
            for (int i = 0; i < grid.Height; i++)
            {
                for (int j = 0; j < grid.Width; j++)
                {
                    if (flags[i][j] == 1)
                        grid.SetState(i, j, tmpStates[i][j]);
                }
            }

            ConsoleManager.PrintElapsedTime("DRAWING");
            ConsoleManager.CaptureTime();
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Computes next generation of this automaton
        /// </summary>
        public void NextGeneration()
        {
            nextGenerationPartial();
            //nextGenerationSeq();
        }

        public void Show()
        {
            grid.Show();
        }
    }
}
