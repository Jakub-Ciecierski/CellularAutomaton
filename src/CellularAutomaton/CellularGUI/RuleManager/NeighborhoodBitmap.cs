using CellularAutomaton.Neighborhoods;
using CellularAutomaton.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CellularGUI.RuleManager
{
    public class NeighborhoodBitmap
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private bool isFirst = true;

        private const int NEUMANN_WIDTH = 3;
        private const int NEUMANN_HEIGHT = 3;

        private const int MOORE_WIDTH = 3;
        private const int MOORE_HEIGHT = 3;

        private const int EXTENDED_MOORE_WIDTH = 5;
        private const int EXTENDED_MOORE_HEIGHT = 5;

        /// <summary>
        ///     The DPI constants
        /// </summary>
        private const double DPI_X = 300.0;
        private const double DPI_Y = 300.0;

        /// <summary>
        ///     Color of the nest
        /// </summary>
        private System.Drawing.Color NEST_COLOR = System.Drawing.Color.FromArgb(200, 20, 20);
        private System.Drawing.Color CENTER_NEST_COLOR = System.Drawing.Color.FromArgb(20, 255, 20);

        /// <summary>
        ///     Dimensions of a cell in pixels
        /// </summary>
        private const int CELL_WIDTH = 10;
        private const int CELL_HEIGHT = 10;

        private int widthPixels;

        public int WidthPixels
        {
            get { return widthPixels; }
            private set { widthPixels = value; }
        }

        private int heightPixels;

        public int HeightPixels
        {
            get { return heightPixels; }
            private set { heightPixels = value; }
        }

        private int stride;
        private int bytesPerPixel;

        /// <summary>
        ///     Actual bitmap of the automaton
        /// </summary>
        private WriteableBitmap wBitmap;

        public WriteableBitmap Bitmap
        {
            get { return wBitmap; }
            set { wBitmap = value; }
        }

        private NeighborhoodType type;

        public NeighborhoodType Type
        {
            get { return type; }
            private set { type = value; }
        }

        private int localCell;

        public int LocalCell
        {
            get { return localCell; }
            private set { localCell = value; }
        }


        private int[] neighbors;
      
        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public NeighborhoodBitmap(NeighborhoodType type, int localCell, int[] neighbors)
        {
            // init() will put states in the oposite value
            this.localCell = localCell;
            this.neighbors = neighbors;
            Type = type;

            init(1,1);

            isFirst = false;
        }

        public NeighborhoodBitmap(int width, int height)
        {
            Type = NeighborhoodType.None;
            localCell = CellStates.DEAD;

            init(width, height);

            isFirst = false;
        }
        
        public NeighborhoodBitmap(NeighborhoodType type)
        {
            localCell = CellStates.DEAD;

            Type = type;
            if (Type == NeighborhoodType.Neumann)
                neighbors = new int[4];
            if (Type == NeighborhoodType.Moore)
                neighbors = new int[8];
            if (Type == NeighborhoodType.ExtendedMoore)
                neighbors = new int[24];
            for (int i = 0; i < neighbors.Count(); i++)
                neighbors[i] = CellStates.DEAD;

            init(0,0);

            isFirst = false;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void initCustom(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    fillCell(i, j);
                    drawNestAroundCell(i, j, NEST_COLOR);
                }
            }
            drawNestAroundCell(0, 0, CENTER_NEST_COLOR);
        }

        private void init(int width, int height)
        {
            switch (Type)
            {
                case NeighborhoodType.Neumann:
                    initBitmap(NEUMANN_WIDTH, NEUMANN_HEIGHT);
                    initNeumann();
                    break;
                case NeighborhoodType.Moore:
                    initBitmap(MOORE_WIDTH, MOORE_HEIGHT);
                    initMoore();
                    break;
                case NeighborhoodType.ExtendedMoore:
                    initBitmap(EXTENDED_MOORE_WIDTH, EXTENDED_MOORE_HEIGHT);
                    initExtendedMoore();
                    break;
                case NeighborhoodType.None:
                    initBitmap(width, height);
                    initCustom(width, height);
                    break;
            }
        }

        private void initNeumann()
        {
            fillCell(0, 1);
            fillCell(1, 0);
            fillCell(1, 1);
            fillCell(1, 2);
            fillCell(2, 1);

            drawNestAroundCell(0, 1, NEST_COLOR);
            drawNestAroundCell(1, 0, NEST_COLOR);
            drawNestAroundCell(1, 2, NEST_COLOR);
            drawNestAroundCell(2, 1, NEST_COLOR);
            drawNestAroundCell(1, 1, CENTER_NEST_COLOR);
        }

        private void initMoore()
        {
            for (int i = 0; i < MOORE_WIDTH; i++)
            {
                for (int j = 0; j < MOORE_HEIGHT; j++)
                {
                    fillCell(i, j);
                    drawNestAroundCell(i, j, NEST_COLOR);
                }
            }
            drawNestAroundCell(1, 1, CENTER_NEST_COLOR);
        }

        private void initExtendedMoore()
        {
            for (int i = 0; i < EXTENDED_MOORE_WIDTH; i++)
            {
                for (int j = 0; j < EXTENDED_MOORE_HEIGHT; j++)
                {
                    fillCell(i, j);
                    drawNestAroundCell(i, j, NEST_COLOR);
                }
            }
            drawNestAroundCell(2, 2, CENTER_NEST_COLOR);
        }

        private void initBitmap(int width, int height)
        {
            int bitmapWidth = width * CELL_WIDTH + width + 1;
            int bitmapHeight = height * CELL_HEIGHT + height + 1;

            wBitmap = new WriteableBitmap(bitmapWidth, bitmapHeight, DPI_X, DPI_Y, PixelFormats.Rgb24, null);

            widthPixels = wBitmap.PixelWidth;
            heightPixels = wBitmap.PixelHeight;
            stride = wBitmap.BackBufferStride;
            bytesPerPixel = (wBitmap.Format.BitsPerPixel) / 8;
        }

        private int setCell(int cellRow, int cellCol)
        {
            int state = 0;
            if (Type == NeighborhoodType.Neumann)
            {
                // North
                if (cellRow == 0 && cellCol == 1)
                {
                    if(isFirst)
                        state = neighbors[0];
                    else
                        state = neighbors[0] = CellStates.GetOpositeState(neighbors[0]);
                }
                    
                // East
                if (cellRow == 1 && cellCol == 2)
                {
                    if(isFirst)
                        state = neighbors[1];
                    else
                        state = neighbors[1] = CellStates.GetOpositeState(neighbors[1]);
                }
                    
                // South
                if (cellRow == 2 && cellCol == 1)
                {
                    if(isFirst)
                        state = neighbors[2];
                    else
                        state = neighbors[2] = CellStates.GetOpositeState(neighbors[2]);
                }
                    
                // West
                if (cellRow == 1 && cellCol == 0)
                {
                    if(isFirst)
                        state = neighbors[3];
                    else
                        state = neighbors[3] = CellStates.GetOpositeState(neighbors[3]);
                }

                if (cellRow == 1 && cellCol == 1)
                {
                    if (isFirst)
                        state = localCell;
                    else
                        state = localCell = CellStates.GetOpositeState(localCell);
                }
                    
            }
            if (Type == NeighborhoodType.Moore)
            {
                if (cellRow == 1 && cellCol == 1)
                {
                    if (isFirst)
                        state = localCell;
                    else
                        state = localCell = CellStates.GetOpositeState(localCell);
                }
                    
                else
                {
                    int index = cellRow * MOORE_WIDTH + cellCol;
                    index = index > 4 ? index - 1 : index;

                    if (isFirst)
                        state = neighbors[index];
                    else
                        state = neighbors[index] = CellStates.GetOpositeState(neighbors[index]);
                }
                
            }
            if (Type == NeighborhoodType.ExtendedMoore)
            {
                if (cellRow == 2 && cellCol == 2)
                {
                    if (isFirst)
                        state = localCell;
                    else
                        state = localCell = CellStates.GetOpositeState(localCell);  
                }
                    
                else
                {
                    int index = cellRow * EXTENDED_MOORE_WIDTH + cellCol;
                    index = index > 12 ? index - 1 : index;

                    if (isFirst)
                        state = neighbors[index];
                    else
                        state = neighbors[index] = CellStates.GetOpositeState(neighbors[index]);
                }
                
            }
            if (Type == NeighborhoodType.None)
            {
                if (isFirst)
                    state = localCell;
                else
                    state = localCell = CellStates.GetOpositeState(localCell);
            }
            return state;
        }

        /// <summary>
        ///     Fills a cell with given color
        /// </summary>
        /// <param name="cellRow"></param>
        /// <param name="cellCol"></param>
        /// <param name="c"></param>
        private unsafe void fillCell(int cellRow, int cellCol)
        {
            if(Type == NeighborhoodType.Neumann && 
                ((cellRow == 0 && cellCol == 0) ||
                (cellRow == 0 && cellCol == 2) ||
                (cellRow == 2 && cellCol == 0) ||
                (cellRow == 2 && cellCol == 2)))
                return;

            int state = setCell(cellRow, cellCol);

            int startPixelI = ((cellRow * CELL_WIDTH) + cellRow + 1);
            int startPixelJ = ((cellCol * CELL_HEIGHT) + cellCol + 1);

            // todo global states

            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            Int32Rect rect = new Int32Rect(startPixelJ, startPixelI, CELL_WIDTH, CELL_HEIGHT);

            byte* startPixel = pImgData +
                                (stride * (startPixelI + 1)) +
                                ((startPixelJ + 1) * (bytesPerPixel));

            System.Drawing.Color c = GridSettings.GetStateColor(state);
            for (int x = 0; x < CELL_WIDTH; x++)
            {
                for (int y = 0; y < CELL_HEIGHT; y++)
                {
                    startPixel = pImgData +
                                (stride * (startPixelI + x)) +
                                (bytesPerPixel * (startPixelJ + y));
                     
                    try
                    {
                        // color the bitmap
                        startPixel[0] = c.R;
                        startPixel[1] = c.G;
                        startPixel[2] = c.B;
                    }
                    catch (AccessViolationException e) { Console.Write(e.StackTrace); }
                }
            }

            try
            {
                wBitmap.AddDirtyRect(rect);
            }
            catch (ArgumentException e) { Console.Write(e.StackTrace); }
            wBitmap.Unlock();

        }

        private void drawNestAroundCell(int cellRow, int cellCol, System.Drawing.Color c)
        {
            int startPixelI = ((cellRow * CELL_WIDTH) + cellRow + 1) - 1;
            int startPixelJ = ((cellCol * CELL_HEIGHT) + cellCol + 1) - 1;

            for (int i = 0; i < CELL_WIDTH + 1; i++)
            {
                startPixelI += 1;
                putPixel(startPixelI, startPixelJ, c);
            }
            
            for (int i = 0; i < CELL_HEIGHT + 1; i++)
            {
                startPixelJ += 1;
                putPixel(startPixelI, startPixelJ, c);
            }
            
            for (int i = 0; i < CELL_WIDTH + 1; i++)
            {
                startPixelI -= 1;
                putPixel(startPixelI, startPixelJ, c);
            }
            
            for (int i = 0; i < CELL_HEIGHT + 1; i++)
            {
                startPixelJ -= 1;
                putPixel(startPixelI, startPixelJ, c);
            }
        }

        private unsafe void putPixel(int i, int j, System.Drawing.Color c)
        {
            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            Int32Rect rect = new Int32Rect(i, j, 1, 1);

            byte* pixel = pImgData +
                                (stride * i) +
                                (j * (bytesPerPixel));
            try
            {
                // color the bitmap
                pixel[0] = c.R;
                pixel[1] = c.G;
                pixel[2] = c.B;
            }
            catch (AccessViolationException e) { Console.Write(e.StackTrace); }

            try
            {
                wBitmap.AddDirtyRect(rect);
            }
            catch (ArgumentException e) { Console.Write(e.StackTrace); }
            wBitmap.Unlock();
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void FillCellByImagePoint(System.Windows.Point point, Image imageDest)
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;

            double actualWidth = imageDest.ActualWidth;
            double actualHeight = imageDest.ActualHeight;

            double scaleWidth = actualWidth / width;
            double scaleHeight = actualHeight / height;

            // scalled pixel indecies image
            int pixelI = (int)(point.X / scaleWidth);
            int pixelJ = (int)(point.Y / scaleHeight);

            // cell indecies
            int cellI = (pixelI - 1) / (CELL_WIDTH + 1);
            int cellJ = (pixelJ - 1) / (CELL_HEIGHT + 1);

            fillCell(cellJ, cellI);
        }

        public Neighborhood ToNeighborhood()
        {
            if (Type == NeighborhoodType.Neumann)
                return new NeumannNeighborhood(localCell, neighbors);
            else if (Type == NeighborhoodType.Moore)
                return new MooreNeighborhood(localCell, neighbors);
            else if (Type == NeighborhoodType.ExtendedMoore)
                return new ExtendedMooreNeighborhood(localCell, neighbors);
            else
                throw new NotImplementedException();
        }
    }
}
