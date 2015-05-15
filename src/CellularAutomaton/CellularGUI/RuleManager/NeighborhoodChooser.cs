using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using CellularGUI.Properties;
using System.Windows.Media;
using CellularAutomaton.Generation;

namespace CellularGUI.RuleManager
{
    public class NeighborhoodChooser
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private const double NEUMANN_SIZE_SCALLAR = 2.5;
        private const double MOORE_SIZE_SCALLAR = 2.5;
        private const double EXTENDEDMOORE_SIZE_SCALLAR = 2.0;

        private const int ROW_COUNT = 4;
        private const int COL_COUNT = 2;
        private const int ROW_HEIGHT = 170;

        private Grid editor;

        public Grid Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        private NeighborhoodBitmap neumannBitmap;

        public NeighborhoodBitmap NeumannBitmap
        {
            get { return neumannBitmap; }
            set { neumannBitmap = value; }
        }

        private NeighborhoodBitmap mooreBitmap;

        public NeighborhoodBitmap MooreBitmap
        {
            get { return mooreBitmap; }
            set { mooreBitmap = value; }
        }

        private NeighborhoodBitmap extendedMooreBitmap;

        public NeighborhoodBitmap ExtendedMooreBitmap
        {
            get { return extendedMooreBitmap; }
            set { extendedMooreBitmap = value; }
        }

        private Image neumannImage;

        public Image NeumannImage
        {
            get { return neumannImage; }
            set { neumannImage = value; }
        }

        private Image neumannSimpleImage;

        public Image NeumannSimpleImage
        {
            get { return neumannSimpleImage; }
            set { neumannSimpleImage = value; }
        }

        private Image mooreImage;

        public Image MooreImage
        {
            get { return mooreImage; }
            set { mooreImage = value; }
        }

        private Image mooreSimpleImage;

        public Image MooreSimpleImage
        {
            get { return mooreSimpleImage; }
            set { mooreSimpleImage = value; }
        }

        private Image extendedMooreImage;

        public Image ExtendedMooreImage
        {
            get { return extendedMooreImage; }
            set { extendedMooreImage = value; }
        }

        private Image extendedMooreSimpleImage;

        public Image ExtendedMooreSimpleImage
        {
            get { return extendedMooreSimpleImage; }
            set { extendedMooreSimpleImage = value; }
        }

        private RuleEditorCycle ruleEditorCycle;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public NeighborhoodChooser(RuleEditorCycle ruleEditorCycle)
        {
            this.ruleEditorCycle = ruleEditorCycle;

            NeumannImage = new Image();
            MooreImage = new Image();
            ExtendedMooreImage = new Image();

            NeumannSimpleImage = new Image();
            MooreSimpleImage = new Image();
            ExtendedMooreSimpleImage = new Image();

            Editor = ruleEditorCycle.Editor;
            init();
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void init()
        {
            initGrid();

            initImages();
        }

        private void initGrid()
        {
            Editor.RowDefinitions.Clear();
            Editor.ColumnDefinitions.Clear();
            Editor.Children.Clear();

            for (int i = 0; i < ROW_COUNT; i++)
            {
                Editor.RowDefinitions.Add(new RowDefinition());
                if(i == 0)
                    Editor.RowDefinitions[i].Height = new System.Windows.GridLength(50  , System.Windows.GridUnitType.Pixel);
                else
                    Editor.RowDefinitions[i].Height = new System.Windows.GridLength(ROW_HEIGHT, System.Windows.GridUnitType.Pixel);
            }

            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = Brushes.White;
            textBlock.Text = "CHOOSE NEIGHBORHOOD";
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontSize = 20.0;

            Editor.Children.Add(textBlock);
            textBlock.SetValue(Grid.RowProperty, 0);
        }

        private void addNeighborhood(NeighborhoodBitmap bitmap, Image detailedImage, Image simpleImage, string text, int gridRow)
        {
            Grid grid = new Grid();

            for (int i = 0; i < 2; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions[i].Height = new System.Windows.GridLength(i * 90 + 30, System.Windows.GridUnitType.Pixel);

                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions[i].Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
            }

            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = Brushes.White;
            textBlock.Text = text;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;

            detailedImage.Source = bitmap.Bitmap;
            detailedImage.Width = bitmap.WidthPixels * EXTENDEDMOORE_SIZE_SCALLAR;
            detailedImage.Height = bitmap.HeightPixels * EXTENDEDMOORE_SIZE_SCALLAR;

            simpleImage.Source = bitmap.Bitmap;
            simpleImage.Width = bitmap.WidthPixels * EXTENDEDMOORE_SIZE_SCALLAR;
            simpleImage.Height = bitmap.HeightPixels * EXTENDEDMOORE_SIZE_SCALLAR;

            Border border = new Border();
            border.BorderThickness = new System.Windows.Thickness(5, 5, 5, 5);
            border.Style = (Style)Application.Current.MainWindow.FindResource("ruleEditorBorder");

            grid.Children.Add(textBlock);
            grid.Children.Add(detailedImage);
            //grid.Children.Add(simpleImage);
            grid.Children.Add(border);

            textBlock.SetValue(Grid.RowProperty, 0);
            textBlock.SetValue(Grid.ColumnSpanProperty, 2);

            detailedImage.SetValue(Grid.RowProperty, 1);
            detailedImage.SetValue(Grid.ColumnSpanProperty, 2);

            simpleImage.SetValue(Grid.RowProperty, 1);
            simpleImage.SetValue(Grid.ColumnProperty, 1);

            border.SetValue(Grid.RowSpanProperty, 2);

            Editor.Children.Add(grid);
            grid.SetValue(Grid.RowProperty, gridRow);
        }

        private void addNeumann()
        {
            NeumannBitmap = new NeighborhoodBitmap(NeighborhoodType.Neumann);

            addNeighborhood(NeumannBitmap, NeumannImage, NeumannSimpleImage, "Neumann Neighborhood", 1);

            NeumannImage.MouseUp += neumannDetailedMouseUp;
            NeumannSimpleImage.MouseUp += neumannSimpleImageMouseUp;
        }

        private void addMoore()
        {
            MooreBitmap = new NeighborhoodBitmap(NeighborhoodType.Moore);

            addNeighborhood(MooreBitmap, MooreImage, MooreSimpleImage, "Moore Neighborhood", 2);

            MooreImage.MouseUp += mooreDetailedMouseUp;
            MooreSimpleImage.MouseUp += mooreSimpleImageMouseUp;
        }


        private void addExtendedMoore()
        {
            ExtendedMooreBitmap = new NeighborhoodBitmap(NeighborhoodType.ExtendedMoore);

            addNeighborhood(ExtendedMooreBitmap, ExtendedMooreImage, ExtendedMooreSimpleImage,"Extended Moore Neighborhood", 3);

            ExtendedMooreImage.MouseUp += extendedMooreDetailedMouseUp;
            extendedMooreSimpleImage.MouseUp += extendedMooreSimpleImageMouseUp;
        }

        private void initImages()
        {
            addNeumann();
            addMoore();
            addExtendedMoore();
        }

        void neumannDetailedMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ruleEditorCycle.RunRuleEditor(NeighborhoodType.Neumann, RuleType.Detailed);
        }

        void neumannSimpleImageMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ruleEditorCycle.RunRuleEditor(NeighborhoodType.Neumann, RuleType.Simple);
        }

        void mooreDetailedMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ruleEditorCycle.RunRuleEditor(NeighborhoodType.Moore, RuleType.Detailed);
        }

        void mooreSimpleImageMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ruleEditorCycle.RunRuleEditor(NeighborhoodType.Moore, RuleType.Simple);
        }

        void extendedMooreDetailedMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ruleEditorCycle.RunRuleEditor(NeighborhoodType.ExtendedMoore, RuleType.Detailed);
        }

        void extendedMooreSimpleImageMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ruleEditorCycle.RunRuleEditor(NeighborhoodType.ExtendedMoore, RuleType.Simple);
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
