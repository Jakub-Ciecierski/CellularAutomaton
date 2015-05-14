﻿using CellularAutomaton;
using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using CellularAutomaton.Settings;
using CellularGUI.RuleManager;
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
using System.Windows.Threading;

namespace CellularGUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

            windowsSettings();
            
            initAutomaton();
        }

        private void windowsSettings()
        {
            //this.ShowTitleBar = false;
        }

        private void initAutomaton()
        {
            cellularGrid = new CellularGrid(gridHeight, gridWidth);
            automaton = new Automaton(cellularGrid);

            automatonBitmap = new AutomatonBitmap(automaton, automatonImage);

            automatonTimer = new AutomatonDispatcher(automaton, speed);

            automaton.CurrentRule = convwaysGameOfLife();
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
            Rule rule = new Rule(NeighborhoodType.Moore);

            
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
            automaton.NextGeneration();
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
                automaton.CurrentRule = ruleEditorCycle.RuleEditor.CurrentRule;
        }

    }
}
