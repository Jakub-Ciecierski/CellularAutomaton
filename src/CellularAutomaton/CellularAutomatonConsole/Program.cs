using CellularAutomaton;
using CellularAutomaton.Generation;
using CellularAutomaton.Neighborhoods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CellularAutomatonConsole
{
    class Program
    {
        const int DEAD = 0;
        const int LIVE = 1;

        static void Main(string[] args)
        {
            CellularAutomaton.Grid grid = createGrid();

            Automaton automaton = new Automaton(grid);
            automaton.CurrentRule = createRule();

            run(automaton);
        }

        static private void run(Automaton automaton)
        {
            automaton.Show();
            Console.Clear();

            while (true)
            {
                automaton.NextGeneration();
                automaton.Show();

                Thread.Sleep(2000);
                Console.Clear();
            }
        }

        static private CellularAutomaton.Grid createGrid()
        {
            CellularAutomaton.Grid grid = new CellularAutomaton.Grid(10, 15, 0);
            grid.SetState(5, 3, 1);
            grid.SetState(5, 4, 1);
            grid.SetState(6, 3, 1);
            return grid;
        }

        static private Rule createRule()
        {
            Rule rule = new Rule(NeighborhoodTypes.Neumann);

            // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
            Transition transition1 = new Transition(DEAD, x => 
                        { 
                            return (x.NeighborCount(LIVE) == 2 && x.LocalCell==LIVE); 
                        });

            // Any live cell with more than three live neighbours dies, as if by overcrowding.
            Transition transition2 = new Transition(LIVE, x =>
            {
                return (x.NeighborCount(LIVE) == 1 && x.LocalCell == DEAD);
            });

            rule.AddTransition(transition1);
            rule.AddTransition(transition2);

            return rule;
        }

        static Rule convwaysGameOfLife()
        {
            Rule rule = new Rule(NeighborhoodTypes.Neumann);

            // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
            Transition transition1 = new Transition(DEAD, x =>
            {
                return (x.NeighborCount(LIVE) < 2 && x.LocalCell == LIVE);
            });

            // Any live cell with more than three live neighbours dies, as if by overcrowding.
            Transition transition2 = new Transition(DEAD, x =>
            {
                return (x.NeighborCount(LIVE) > 3 && x.LocalCell == LIVE);
            });

            // Any live cell with more than three live neighbours dies, as if by overcrowding.
            Transition transition3 = new Transition(LIVE, x =>
            {
                return (x.NeighborCount(LIVE) == 3 && x.LocalCell == DEAD);
            });

            // Any live cell with two or three live neighbours lives on to the next generation.
            Transition transition4 = new Transition(LIVE, x =>
            {
                return ((x.NeighborCount(LIVE) == 2 || x.NeighborCount(LIVE) == 3) && x.LocalCell == LIVE);
            });

            rule.AddTransition(transition1);
            rule.AddTransition(transition2);
            rule.AddTransition(transition3);
            rule.AddTransition(transition4);

            return rule;
        }
    }
}
