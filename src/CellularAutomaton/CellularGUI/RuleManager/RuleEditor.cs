using CellularAutomaton;
using CellularAutomaton.Generation;
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
using System.Windows.Shapes;

namespace CellularGUI.RuleManager
{
    public class RuleEditor
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private const double NEIGHBORHOOD_SIZE_SCALLAR = 3.0;
        private const double NEWSTATE_SIZE_SCALLAR = 1.0;

        private const string BACKGROUND_COLOR = "#FF2C2C2C";

        private const int ROW_COUNT = 4;

        private ListView ruleListView;

        private TextBlock nameTextBlock;

        private TextBox nameTextBox;

        private Button newTransitionButton;

        private NeighborhoodBitmap neighborhoodBitmap;
        private NeighborhoodBitmap newStateBitmap;

        private Image neighborhoodImage;
        private Image newStateImage;

        private Rule currentRule;

        public Rule CurrentRule
        {
            get { return currentRule; }
            set { currentRule = value; }
        }

        private RuleType ruleType;

        private NeighborhoodType neighborhoodType;

        public NeighborhoodType NeighborhoodType
        {
            get { return neighborhoodType; }
            set { neighborhoodType = value; }
        }

        private Grid editor;

        public Grid Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public RuleEditor(RuleEditorCycle ruleEditorCycle, Rule rule)
        {
            CurrentRule = rule;
            Editor = ruleEditorCycle.Editor;

            init();
        }

        /// <summary>
        ///     Used to create new rule window
        /// </summary>
        /// <param name="type"></param>
        public RuleEditor(RuleEditorCycle ruleEditorCycle, NeighborhoodType neighborhoodType, RuleType ruleType)
        {
            this.ruleType = ruleType;
            NeighborhoodType = neighborhoodType;

            CurrentRule = new Rule(NeighborhoodType);

            Editor = ruleEditorCycle.Editor;

            init();
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void init()
        {
            initGrid();
            initNameComponents();
            initListView();
            initAddButton();
            initTransitionImage();
        }

        private void initGrid()
        {
            Editor.Children.Clear();
            Editor.RowDefinitions.Clear();
            Editor.ColumnDefinitions.Clear();

            for (int i = 0; i < ROW_COUNT; i++)
            {
                Editor.RowDefinitions.Add(new RowDefinition());
            }
            // Name
            Editor.RowDefinitions[0].Height = new System.Windows.GridLength(50, System.Windows.GridUnitType.Pixel);
            // ListView
            Editor.RowDefinitions[1].Height = new System.Windows.GridLength(550, System.Windows.GridUnitType.Pixel);
            // Add new Trainsition
            Editor.RowDefinitions[2].Height = new System.Windows.GridLength(50, System.Windows.GridUnitType.Pixel);
            // Transition view
            Editor.RowDefinitions[3].Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);


            Editor.ColumnDefinitions.Add(new ColumnDefinition());
            Editor.ColumnDefinitions.Add(new ColumnDefinition());

            Editor.ColumnDefinitions[0].Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
            Editor.ColumnDefinitions[1].Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
        }

        private void initNameComponents()
        {
            nameTextBlock = new TextBlock();
            nameTextBlock.Foreground = Brushes.White;
            nameTextBlock.Text = "Name: ";
            nameTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            nameTextBlock.VerticalAlignment = VerticalAlignment.Center;

            nameTextBox = new TextBox();
            nameTextBox.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(BACKGROUND_COLOR));
            nameTextBox.BorderBrush = Brushes.Black;
            nameTextBox.BorderThickness = new Thickness(2, 2, 2, 2);
            nameTextBox.Foreground = Brushes.White;
            nameTextBox.Text = "Rule";
            nameTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            nameTextBox.VerticalAlignment = VerticalAlignment.Center;
            nameTextBox.Margin = new Thickness(40, 0, 0, 0);

            Editor.Children.Add(nameTextBlock);
            Editor.Children.Add(nameTextBox);
            nameTextBlock.SetValue(Grid.RowProperty, 0);
            nameTextBox.SetValue(Grid.RowProperty, 0);

            nameTextBlock.SetValue(Grid.ColumnSpanProperty, 2);
            nameTextBox.SetValue(Grid.ColumnSpanProperty, 2);
        }

        private void initListView()
        {
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            ruleListView = new ListView();
            ruleListView.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(BACKGROUND_COLOR));
            ruleListView.SelectionChanged += ruleListView_SelectionChanged;
            ruleListView.SelectionMode = SelectionMode.Single;

            scrollViewer.Content = ruleListView;

            Editor.Children.Add(scrollViewer);
            
            scrollViewer.SetValue(Grid.RowProperty, 1);
            scrollViewer.SetValue(Grid.ColumnSpanProperty, 2);
        }

        private void initAddButton()
        {
            newTransitionButton = new Button();
            newTransitionButton.Content = "Add new transition";
            newTransitionButton.Click += newTransitionButton_Click;

            Editor.Children.Add(newTransitionButton);
            newTransitionButton.SetValue(Grid.RowProperty, 2);
            newTransitionButton.SetValue(Grid.ColumnSpanProperty, 2);
        }

        private void initTransitionImage()
        {
            neighborhoodImage = new Image();
            neighborhoodBitmap = new NeighborhoodBitmap(NeighborhoodType);
            neighborhoodImage.Source = neighborhoodBitmap.Bitmap;
            neighborhoodImage.Width = neighborhoodBitmap.WidthPixels * NEIGHBORHOOD_SIZE_SCALLAR;
            neighborhoodImage.Height = neighborhoodBitmap.HeightPixels * NEIGHBORHOOD_SIZE_SCALLAR;

            newStateImage = new Image();
            newStateBitmap = new NeighborhoodBitmap(1, 1);
            newStateImage.Source = newStateBitmap.Bitmap;
            newStateImage.Width = neighborhoodBitmap.WidthPixels * NEWSTATE_SIZE_SCALLAR;
            newStateImage.Height = neighborhoodBitmap.HeightPixels * NEWSTATE_SIZE_SCALLAR;

            Editor.Children.Add(neighborhoodImage);
            Editor.Children.Add(newStateImage);

            neighborhoodImage.SetValue(Grid.RowProperty, 3);
            newStateImage.SetValue(Grid.RowProperty, 3);
            neighborhoodImage.SetValue(Grid.ColumnProperty, 0);
            newStateImage.SetValue(Grid.ColumnProperty, 1);

            neighborhoodImage.MouseUp += neighborhoodImage_MouseUp;
            newStateImage.MouseUp += newStateImage_MouseUp;

            /*
            Polygon polygon = new Polygon();

            PointCollection pointCollection = new PointCollection();
            pointCollection.Add(new Point(0,0));
            pointCollection.Add(new Point(0,30));
            pointCollection.Add(new Point(0,10));
            pointCollection.Add(new Point(30,10));
            pointCollection.Add(new Point(30,-10));
            pointCollection.Add(new Point(45,10));
            pointCollection.Add(new Point(30,30));
            pointCollection.Add(new Point(30,20));
            pointCollection.Add(new Point(0,20));
            pointCollection.Add(new Point(0,0));
            pointCollection.Add(new Point(30,0));
            pointCollection.Add(new Point(30,10));
            pointCollection.Add(new Point(0,10));
            polygon.Points = pointCollection;

            Button button = new Button();
            button.Background = Brushes.White;
            button.Content = polygon;

            Editor.Children.Add(button);
            button.SetValue(Grid.RowProperty, 3);
            button.SetValue(Grid.ColumnProperty, 1);
             * */
        }

        void newStateImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
            newStateBitmap.FillCellByImagePoint(point, newStateImage);
        }

        void neighborhoodImage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
            neighborhoodBitmap.FillCellByImagePoint(point, neighborhoodImage);
        }

        private void newTransitionButton_Click(object sender, RoutedEventArgs e)
        {
            Neighborhood nb = neighborhoodBitmap.ToNeighborhood();

            int newState = newStateBitmap.LocalCell;

            if (ruleType == RuleType.Detailed)
            {
                DetailedTransition transition = new DetailedTransition(nb, newState);
                CurrentRule.AddTransition(transition);
            }

            Image image1 = new Image();
            Image image2 = new Image();
            image1.Width = 50;
            image1.Height = 50;

            image2.Width = 15;
            image2.Height = 15;

            image1.Source = neighborhoodImage.Source;
            image2.Source = newStateImage.Source;

            StackPanel stackPanel = new StackPanel();

            stackPanel.Margin = new Thickness(15, 15, 15, 15);
            stackPanel.Orientation = Orientation.Horizontal;

            stackPanel.Children.Add(image1);
            stackPanel.Children.Add(image2);

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Content = stackPanel;
            listViewItem.Foreground = Brushes.White;

            ruleListView.Items.Add(listViewItem);

            initTransitionImage();
        }

        void ruleListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            ListViewItem list = this.ruleListView.SelectedItems[0] as ListViewItem;

            StackPanel stackPanel = list.Content as StackPanel;

            Image image1 = stackPanel.Children[0] as Image;
            Image image2 = stackPanel.Children[1] as Image;

            this.neighborhoodImage.Source = image1.Source;
            this.newStateImage.Source = image2.Source;
             * */
        }


        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
