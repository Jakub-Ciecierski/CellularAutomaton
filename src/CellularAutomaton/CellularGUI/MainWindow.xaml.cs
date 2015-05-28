using CellularAutomaton;
using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using CellularAutomaton.Settings;
using CellularGUI.RuleManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Windows.Threading;

namespace CellularGUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string FILE_EXTENTION = ".pt";

        /// <summary>
        ///     Checks if mouse is being draged
        /// </summary>
        bool isLeftDrag = false;
        bool isRightDrag = false;

        bool isMiddleDrag = false;
        const double middleDragFactor = 0.5;
        System.Windows.Point middleDragStartPoint;

        int gridWidth = 100;
        int gridHeight = 100;

        const int MAX_WIDTH = 500;
        const int MAX_HEIGHT = 500;

        CellularGrid cellularGrid;
        Automaton automaton;

        AutomatonBitmap automatonBitmap;

        private int speed = 20;

        AutomatonDispatcher automatonTimer;

        private const double zoomFactor = 0.1;
        private double zoomValue = 1.0;

        bool isMaximized = false;
        private Rect _restoreLocation;

        private RuleEditorCycle ruleEditorCycle;

        public MainWindow()
        {
            InitializeComponent();

            ruleEditorCycle = new RuleEditorCycle(this.ruleEditorMainGrid);
            widthTextBox.Text = gridWidth.ToString();
            heightTextBox.Text = gridHeight.ToString();

            windowsSettings();
            
            initAutomaton();

            initDataContext();
        }

        private void initDataContext()
        {
            /*
            this.DataContext = new 
            {
                automatonTimer, automaton.CurrentRule 
            };
             * */

            this.DataContext = automatonTimer;
        }

        private void windowsSettings()
        {
            //this.ShowTitleBar = false;
        }

        private void initAutomaton()
        {
            gridWidth = Convert.ToInt32(widthTextBox.Text);
            gridHeight = Convert.ToInt32(heightTextBox.Text);

            if (gridWidth > MAX_WIDTH)
            {
                widthTextBox.Text = MAX_WIDTH.ToString();
            }
            if (gridHeight > MAX_HEIGHT)
            {
                heightTextBox.Text = MAX_HEIGHT.ToString();
            }

            cellularGrid = new CellularGrid(gridHeight, gridWidth);
            automaton = new Automaton(cellularGrid);

            initAutomatonComponents();
        }

        private void initAutomatonComponents()
        {
            automatonBitmap = new AutomatonBitmap(automaton, automatonImage);

            automatonTimer = new AutomatonDispatcher(automaton, speed);

            //applyRule(convwaysGameOfLife());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /*******************************************************************/
        /************************* DRAG AND DRAW ***************************/
        /*******************************************************************/
        private void automatonImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isLeftDrag = true;
        }

        private void automatonImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isLeftDrag = false;
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
            automatonBitmap.FillCellByImagePoint(point, 1);
        }

        private void automatonImage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            isRightDrag = true;
        }

        private void automatonImage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isRightDrag = false;
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
            automatonBitmap.FillCellByImagePoint(point, 0);
        }

        private void automatonImage_MouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(e.Source as FrameworkElement);
            // Draw alive cell
            if (isLeftDrag)
            {
                automatonBitmap.FillCellByImagePoint(point, 1);
            }
            // Draw dead cell
            else if (isRightDrag)
            {
                automatonBitmap.FillCellByImagePoint(point, 0);
            }
            // Move scrollviewer
            else if (isMiddleDrag)
            {
                double deltaX;
                double deltaY;

                deltaX = middleDragStartPoint.X - point.X;
                deltaY = middleDragStartPoint.Y - point.Y;

                gridScollViewer.ScrollToHorizontalOffset(gridScollViewer.HorizontalOffset + deltaX * middleDragFactor);
                gridScollViewer.ScrollToVerticalOffset(gridScollViewer.VerticalOffset + deltaY * middleDragFactor);
            }
        }


        /*******************************************************************/
        /************************* MIDDLE MOUSE ****************************/
        /*******************************************************************/

        private void automatonImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                isMiddleDrag = true;
                middleDragStartPoint = e.GetPosition(e.Source as FrameworkElement);
            }
        }

        private void automatonImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                isMiddleDrag = false;
            }
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double lastZoomValue = zoomValue;
            if (e.Delta > 0)
            {
                zoomValue += zoomFactor;
            }
            else
            {
                zoomValue -= zoomFactor;
            }

            if(!automatonBitmap.ScaleImage(zoomValue))
                zoomValue = lastZoomValue;
        }


        /*******************************************************************/
        /**************************** BUTTONS ******************************/
        /*******************************************************************/

        private void generationButton_Click(object sender, RoutedEventArgs e)
        {
            if(automaton.CurrentRule != null)
                automatonTimer.Start();
        }

        private void stopGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            automatonTimer.Stop();
        }

        /*******************************************************************/
        /**************************** COMMON *******************************/
        /*******************************************************************/

        private Rule convwaysGameOfLife()
        {
            Rule rule = new Rule(NeighborhoodType.Moore, RuleType.Detailed);
            rule.Name = "Convways";
            
            int[] transitionMap1 = { CellStates.ALIVE, -1, 0 };
            SimpleTransition transition1 = new SimpleTransition(transitionMap1, CellStates.DEAD);
            int[] transitionMap2 = { CellStates.ALIVE, -1, 1 };
            SimpleTransition transition2 = new SimpleTransition(transitionMap2, CellStates.DEAD);

            int[] transitionMap3 = { CellStates.ALIVE, -1, 2 };
            SimpleTransition transition3 = new SimpleTransition(transitionMap3, CellStates.ALIVE);
            int[] transitionMap4 = { CellStates.ALIVE, -1, 3 };
            SimpleTransition transition4 = new SimpleTransition(transitionMap4, CellStates.ALIVE);

            int[] transitionMap5 = { CellStates.ALIVE, -1, 4 };
            SimpleTransition transition5 = new SimpleTransition(transitionMap5, CellStates.DEAD);
            int[] transitionMap6 = { CellStates.ALIVE, -1, 5 };
            SimpleTransition transition6 = new SimpleTransition(transitionMap6, CellStates.DEAD);
            int[] transitionMap7 = { CellStates.ALIVE, -1, 6 };
            SimpleTransition transition7 = new SimpleTransition(transitionMap7, CellStates.DEAD);
            int[] transitionMap8 = { CellStates.ALIVE, -1, 7 };
            SimpleTransition transition8 = new SimpleTransition(transitionMap8, CellStates.DEAD);
            int[] transitionMap9 = { CellStates.ALIVE, -1, 8 };
            SimpleTransition transition9 = new SimpleTransition(transitionMap9, CellStates.DEAD);

            int[] transitionMap10 = { CellStates.DEAD, -1, 3 };
            SimpleTransition transition10 = new SimpleTransition(transitionMap10, CellStates.ALIVE);

            /*
            // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
            Transition transition1 = new Transition(CellStates.DEAD, x =>
            {
                return (x.NeighborCount(CellStates.ALIVE) < 2 && x.LocalCell == CellStates.ALIVE);
            });

            // Any live cell with two or three live neighbours lives on to the next generation.
            Transition transition2 = new Transition(CellStates.ALIVE, x =>
            {
                return ((x.NeighborCount(CellStates.ALIVE) == 2 || x.NeighborCount(CellStates.ALIVE) == 3) && x.LocalCell == CellStates.ALIVE);
            });

            // Any live cell with more than three live neighbours dies, as if by overcrowding.
            Transition transition3 = new Transition(CellStates.DEAD, x =>
            {
                return (x.NeighborCount(CellStates.ALIVE) > 3 && x.LocalCell == CellStates.ALIVE);
            });

            // Any live cell with more than three live neighbours dies, as if by overcrowding.
            Transition transition4 = new Transition(CellStates.ALIVE, x =>
            {
                return (x.NeighborCount(CellStates.ALIVE) == 3 && x.LocalCell == CellStates.DEAD);
            });
             * */
            /*
            for (int i = 0; i < 100; i++)
            {
                Transition transition = new Transition(CellStates.ALIVE, x =>
                {
                    return (x.NeighborCount(CellStates.ALIVE) == 3 && x.LocalCell == CellStates.DEAD);
                });
                rule.AddTransition(transition);
            }*/

            rule.AddTransition(transition1);
            rule.AddTransition(transition2);
            rule.AddTransition(transition3);
            rule.AddTransition(transition4);
            rule.AddTransition(transition5);
            rule.AddTransition(transition6);
            rule.AddTransition(transition7);
            rule.AddTransition(transition8);
            rule.AddTransition(transition9);
            rule.AddTransition(transition10);


            return rule;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (!isMaximized)
                    MaximizeWindow();
                else
                    Restore();
            }
            else
            {
                DragMove();
            }
        }

        private void MaximizeWindow()
        {
            isMaximized = true;
            _restoreLocation = new Rect { Width = Width, Height = Height, X = Left, Y = Top };

            System.Windows.Forms.Screen currentScreen;
            currentScreen = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position);

            Height = currentScreen.WorkingArea.Height + 3;
            Width = currentScreen.WorkingArea.Width + 3;

            Left = currentScreen.WorkingArea.X - 2;
            Top = currentScreen.WorkingArea.Y - 2;
        }

        private void Restore()
        {
            isMaximized = false;
            Height = _restoreLocation.Height;
            Width = _restoreLocation.Width;
            Left = _restoreLocation.X;
            Top = _restoreLocation.Y;
        }

        private void stepGenerationButton_Click(object sender, RoutedEventArgs e)
        {
            if (automaton.CurrentRule == null)
                return;
            automaton.NextGeneration();
        }

        private void applyRule(Rule rule)
        {
            if (rule == null)
                return;
            automaton.CurrentRule = rule;
            currentRuleTextBlock.Text = rule.Name;
        }

        private void loadPattern(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            try
            {
                automaton = (Automaton)formatter.Deserialize(stream);
            }
            catch (InvalidCastException ex) { return; }

            stream.Close();

            zoomValue = 1.0;

            widthTextBox.Text = automaton.Grid.Width.ToString();
            heightTextBox.Text = automaton.Grid.Height.ToString();
            if (automaton.CurrentRule != null)
                currentRuleTextBlock.Text = automaton.CurrentRule.Name;
            else
                currentRuleTextBlock.Text = "None";
            patternNameTextBox.Text = automaton.Name;

            initAutomatonComponents();
        }

        /// <summary>
        ///     Starts adding new rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newRuleButton_Click(object sender, RoutedEventArgs e)
        {
            ruleEditorCycle.RunNeighborhoodChooser();
        }

        private void applyRuleButton_Click(object sender, RoutedEventArgs e)
        {
            if (ruleEditorCycle.RuleEditor != null)
                applyRule(ruleEditorCycle.RuleEditor.CurrentRule);
        }

        private void saveRuleButton_Click(object sender, RoutedEventArgs e)
        {
            if (ruleEditorCycle.RuleEditor != null)
                ruleEditorCycle.RuleEditor.SafeRuleToFile();
        }

        private void newAutomatonButton_Click(object sender, RoutedEventArgs e)
        {
            automatonTimer.Stop();
            initAutomaton();
        }

        private void savePatternButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                string name = path + "/" + patternNameTextBox.Text + FILE_EXTENTION;
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(name, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, automaton);
                stream.Close();
            }
        }

        private void editRuleButton_Click(object sender, RoutedEventArgs e)
        {
            ruleEditorCycle.RuleEditor.StartEditRule(automaton.CurrentRule);
        }

        private void ruleDropFileHandler(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            ruleEditorCycle.RuleEditor.StartLoadRule(filenames[0]);
            
        }
        private void automatonDropFileHandler(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            loadPattern(filenames[0]);

        }
        private void loadPattern_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Pattern"; // Default file name
            dlg.DefaultExt = ".pt"; // Default file extension
            dlg.Filter = "Pattern (*.pt) | *.pt";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                loadPattern(filename); 
            }
        }

        private void loadRule_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Rule"; // Default file name
            dlg.DefaultExt = ".rl"; // Default file extension
            dlg.Filter = "Rule (*.rl) | *.rl";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                ruleEditorCycle.RuleEditor.StartLoadRule(filename);
            }
        }

        
        private void showGuide_Click(object sender, RoutedEventArgs e)
        {
            //guideGridSplitter.
        }

        
        void patternTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(automaton != null)
                automaton.Name = patternNameTextBox.Text;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            //SystemCommands.MaximizeWindow(this);
            if (!isMaximized)
                MaximizeWindow();
            else
                Restore();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
            
    }
}
