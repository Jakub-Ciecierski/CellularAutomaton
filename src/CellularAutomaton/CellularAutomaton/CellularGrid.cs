using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton
{
    /// <summary>
    ///     Grid of cells used in automaton
    /// </summary>
    public class CellularGrid
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     The matrix of cells
        /// </summary>
        private List<List<int>> gridMatrix = new List<List<int>>();

        /// <summary>
        ///     The width of the grid
        /// </summary>
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        ///     The height of the grid
        /// </summary>
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        ///     Enables/Disables the Wrapping feature:
        ///     Wrapping imitates infinite sized grid.
        ///     Neighborhood of cell on right edge will
        ///     include the cells on the left side of the grid.
        /// </summary>
        private bool wrapping;

        public bool Wrapping
        {
            get { return wrapping; }
            set { wrapping = value; }
        }

        /// <summary>
        ///     Pointer to drawing function
        /// </summary>
        private Action<int, int, int> drawCell;

        public Action<int, int, int> DrawCell
        {
            get { return drawCell; }
            set { drawCell = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        /// <summary>
        ///     Creates an empty grid
        /// </summary>
        public CellularGrid()
        {
            Wrapping = true;
        }

        /// <summary>
        ///     Creates grid with specified dimensions
        /// </summary>
        /// <param name="width">
        ///     Width of the grid
        /// </param>
        /// <param name="height">
        ///     Height of the grid
        /// </param>
        public CellularGrid(int height, int width)
        {
            Height = height;
            Width = width;

            Wrapping = true;

            initGrid(0);
        }

        /// <summary>
        ///     Creates grid with specified dimensions, and
        ///     sets the initial state of cells with specified state
        /// </summary>
        /// <param name="width">
        ///     Width of the grid
        /// </param>
        /// <param name="height">
        ///     Height of the grid
        /// </param>
        /// <param name="initState">
        ///     The state which all the cells in the grid
        ///     should be initialized with
        /// </param>
        public CellularGrid(int height, int width, int initState)
        {
            Height = height;
            Width = width;

            Wrapping = true;

            initGrid(initState);
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Initializes the grid
        /// </summary>
        private void initGrid(int initState)
        {
            for (int i = 0; i < Height; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < Width; j++)
                {
                    row.Add(initState);
                }
                gridMatrix.Add(row);
            }
        }

        private NeumannNeighborhood getNeumannNeighborhood(int i, int j)
        {
            int localCell = GetState(i, j);
            
            // Get upper neighbor
            int tmpI = (i-1 < 0) ? Height-1 : i-1;
            int tmpJ = j;
            int upper = GetState(tmpI, tmpJ);

            // Get right neighbor
            tmpI = i;
            tmpJ = (j + 1 > Width - 1) ? 0 : j + 1;
            int right = GetState(tmpI, tmpJ);

            // Get lower neighbor
            tmpI = (i + 1 > Height-1) ? 0 : i + 1;
            tmpJ = j;
            int bottom = GetState(tmpI, tmpJ);

            // Get left neighbor
            tmpI = i;
            tmpJ = (j - 1 < 0) ? Width -1 : j - 1;
            int left = GetState(tmpI, tmpJ);

            int[] neighbors = { upper, right, bottom, left };

            NeumannNeighborhood nb = new NeumannNeighborhood(localCell, neighbors);
            return nb;
        }

        private MooreNeighborhood getMooreNeighborhood(int i, int j)
        {
            int localCell = GetState(i, j);
            int ni = 0;
            int[] neighbors = new int[8];

            for (int k = -1; k <= 1; k++)
            {
                for (int l = -1; l <= 1; l++)
                {
                    if(!(k ==0 && l == 0))
                        neighbors[ni++] = GetState(i + k, j + l);
                }
            }

            return new MooreNeighborhood(localCell, neighbors);

        }

        private ExtendedMooreNeighborhood getExtendedMooreNeighborhood(int i, int j)
        {
            int localCell = GetState(i, j);
            int ni = 0;
            int[] neighbors = new int[24];

            for (int k = -2; k <= 2; k++)
            {
                for (int l = -2; l <= 2; l++)
                {
                    if (!(k == 0 && l == 0))
                        neighbors[ni++] = GetState(i + k, j + l);
                }
            }

            return new ExtendedMooreNeighborhood(localCell, neighbors);
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Computes a neighborhood a given cell in a given neighborhood type
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="nbType"></param>
        /// <returns>
        ///     Neighborhood of a given cell
        /// </returns>
        public Neighborhood GetNeighborhood(int i, int j, NeighborhoodTypes nbType)
        {
            if (nbType == NeighborhoodTypes.Neumann)
                return getNeumannNeighborhood(i, j);
            else if (nbType == NeighborhoodTypes.Moore)
                return getMooreNeighborhood(i, j);
            else if (nbType == NeighborhoodTypes.ExtendedMoore)
                return getExtendedMooreNeighborhood(i, j);
            else
                throw new NotImplementedException("Neighborhood not implemented");
        }

        public int GetState(int i, int j)
        {
            int wrapI = i;
            int wrapJ = j;
            
            // If wrapping is enabled, take indecies from opposite edges
            if (Wrapping)
            {
                wrapI = (wrapI < 0) ? Height + wrapI : wrapI;
                wrapI = (wrapI > Height - 1) ? wrapI - Height : wrapI;

                wrapJ = (wrapJ < 0) ? Width + wrapJ : wrapJ;
                wrapJ = (wrapJ > Width - 1) ? wrapJ - Width : wrapJ;
            }

            List<int> row;
            lock (gridMatrix)
            {
                row = gridMatrix.ElementAt(wrapI);
            }
            return row[wrapJ];
        }

        /// <summary>
        ///     Sets new state to cell
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="state"></param>
        public void SetState(int i, int j, int state)
        {
            lock (gridMatrix)
            {
                List<int> row = gridMatrix.ElementAt(i);
                row[j] = state;
            }
            if(DrawCell != null)
                DrawCell(i, j, state);
        }

        /// <summary>
        ///     Prints the grid to console
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < Height; i++)
            {
                List<int> row = gridMatrix.ElementAt(i);
                Console.Write("| ");
                for (int j = 0; j < Width; j++)
                {
                    Console.Write(row.ElementAt(j) + " ");
                }
                Console.Write("|\n");
            }
        }
    }
}
