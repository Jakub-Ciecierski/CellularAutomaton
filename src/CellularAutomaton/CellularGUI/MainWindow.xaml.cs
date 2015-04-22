using CellularAutomaton;
using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CellularAutomaton.Grid grid = createGrid();

            Automaton automaton = new Automaton(grid);
            automaton.CurrentRule = createRule();

            run(automaton);
        }

        private void run(Automaton automaton)
        {
            while (true)
            {
                automaton.NextGeneration();
                automaton.Show();

                Thread.Sleep(1500);
                Console.Clear();
            }
        }

        private CellularAutomaton.Grid createGrid()
        {
            CellularAutomaton.Grid grid = new CellularAutomaton.Grid(10, 15, 0);
            grid.SetState(5, 3, 1);
            grid.Show();
            return grid;
        }

        private Rule createRule()
        {
            Rule rule = new Rule(NeighborhoodTypes.Neumann);
            
            // dies if has 2 neighbors alive
            Transition transition1 = new Transition(0, x => { return (x.NeighborCount(1) == 2); });

            // is born if has 3 neighbors dead
            Transition transition2 = new Transition(1, x => { return (x.NeighborCount(0) == 3); });

            rule.AddTransition(transition1);
            rule.AddTransition(transition2);

            return rule;
        }
    }
}
