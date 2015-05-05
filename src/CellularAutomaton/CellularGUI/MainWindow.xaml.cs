using CellularAutomaton;
using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CellularGUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Drawing.Color nestColor = System.Drawing.Color.FromArgb(200, 20, 20);

        /// <summary>
        ///     Checks if mouse is being draged
        /// </summary>
        bool isDragAlive = false;
        bool isDragDead = false;

        const int cellWidth = 5;
        const int cellHeight = 5;

        int gridWidth = 500;
        int gridHeight = 500;

        CellularGrid cellularGrid;
        Automaton automaton;

        WriteableBitmap wBitmap;

        public MainWindow()
        {
            InitializeComponent();
            initGrid();
            initBitmap();
        }

        private void initGrid()
        {
            // Make space for each cell - which is of size (cellWidth x cellHeigh),
            // and for the nest
            int bitmapWidth = gridWidth * cellWidth + gridWidth + 1;
            int bitmapHeight = gridHeight * cellHeight + gridHeight + 1;

            wBitmap = new WriteableBitmap(bitmapWidth, bitmapHeight, 300, 300, PixelFormats.Rgb24, null);
            automatonImage.Source = wBitmap;

            cellularGrid = new CellularGrid(gridHeight,gridWidth);
            automaton = new Automaton(cellularGrid);
        }

        private unsafe void initBitmap()
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;
            int stride = wBitmap.BackBufferStride;
            int bytesPerPixel = (wBitmap.Format.BitsPerPixel) / 8;

            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            int cRowStart = 0;
            int cColStart = 0;
            for (int row = 0; row < height; row++)
            {
                cColStart = cRowStart;
                for (int col = 0; col < width; col++)
                {
                    byte* bPixel = pImgData + cColStart;

                    // draw the nest
                    if ((col % (cellWidth + 1) == 0) || (row % (cellHeight + 1) == 0) || col == width - 1 || row == height - 1)
                    {
                        bPixel[0] = nestColor.R;
                        bPixel[1] = nestColor.G;
                        bPixel[2] = nestColor.B;
                    }
                    // draw the cell
                    else
                    {
                        bPixel[0] = automaton.DeadColor.R;
                        bPixel[1] = automaton.DeadColor.G;
                        bPixel[2] = automaton.DeadColor.B;
                    }
                    cColStart += bytesPerPixel;
                }
                cRowStart += stride;
            }
            Int32Rect rect = new Int32Rect(0, 0, width, height);
            wBitmap.AddDirtyRect(rect);
            wBitmap.Unlock();
        }

        /// <summary>
        ///     Fills a cell with given color
        /// </summary>
        /// <param name="cellRow"></param>
        /// <param name="cellCol"></param>
        /// <param name="c"></param>
        private unsafe void fillCell(int cellRow, int cellCol, System.Drawing.Color c)
        {
            int startPixelI = ((cellRow * cellWidth) + cellRow + 1);
            int startPixelJ = ((cellCol * cellHeight) + cellCol + 1);

            // todo global states
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;
            int stride = wBitmap.BackBufferStride;
            int bytesPerPixel = (wBitmap.Format.BitsPerPixel) / 8;

            wBitmap.Lock();
            byte* pImgData = (byte*)wBitmap.BackBuffer;

            Int32Rect rect = new Int32Rect(startPixelJ, startPixelI, cellWidth, cellHeight);

            byte* startPixel = pImgData + 
                                (stride * (startPixelI + 1)) + 
                                ((startPixelJ + 1) * (bytesPerPixel));

            for (int x = 0; x < cellWidth; x++)
            {
                for (int y = 0; y < cellHeight; y++)
                {
                    startPixel = pImgData +
                                (stride * (startPixelI + x)) +
                                (bytesPerPixel * (startPixelJ + y));

                    startPixel[0] = c.R;
                    startPixel[1] = c.G;
                    startPixel[2] = c.B;
                }
            }

            wBitmap.AddDirtyRect(rect);
            wBitmap.Unlock();
        }

        /// <summary>
        ///     Takes point from the image
        ///     Scales it to proper index for bitmap
        ///     and fills a proper cell with given color
        /// </summary>
        /// <param name="point"></param>
        /// <param name="c"></param>
        private void fillCellByImagePoint(System.Windows.Point point, System.Drawing.Color c)
        {
            int width = wBitmap.PixelWidth;
            int height = wBitmap.PixelHeight;

            double actualWidth = automatonImage.ActualWidth;
            double actualHeight = automatonImage.ActualHeight;

            double scaleWidth = actualWidth / width;
            double scaleHeight = actualHeight / height;

            // scalled indecies image
            int pixelI = (int)(point.X / scaleWidth);
            int pixelJ = (int)(point.Y / scaleHeight);

            // pixel indecies
            int cellI = (pixelI - 1) / (cellWidth + 1);
            int cellJ = (pixelJ - 1) / (cellHeight + 1);

            // which cell
            fillCell(cellJ, cellI, c);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /******************** Drag and Draw feature *************************/
        private void automatonImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragAlive = true;
        }

        private void automatonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragAlive = false;
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
            fillCellByImagePoint(point, automaton.AliveColor);
        }

        private void automatonImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragAlive)
            {
                System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
                fillCellByImagePoint(point, automaton.AliveColor);
            }
            if (isDragDead)
            {
                System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
                fillCellByImagePoint(point, automaton.DeadColor);
            }
        }
        private void automatonImage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragDead = true;
        }

        private void automatonImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragDead = false;
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
            fillCellByImagePoint(point, automaton.DeadColor);
        }

        /******************** END Drag and Draw feature *************************/
    }
}
