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
            CellularAutomaton.CellularGrid grid = createGrid();

            Automaton automaton = new Automaton(grid);
            automaton.CurrentRule = convwaysGameOfLife();

            run(automaton);
        }

        static private void run(Automaton automaton)
        {
            automaton.Show();
            Console.Read();
            //Thread.Sleep(500);
            Console.Clear();

            while (true)
            {
                automaton.NextGeneration();
                automaton.Show();

                Console.Read();
                //Thread.Sleep(5000);
                Console.Clear();
            }
        }

        static private CellularAutomaton.CellularGrid createGrid()
        {
            CellularAutomaton.CellularGrid grid = new CellularAutomaton.CellularGrid(10, 15, 0);
            
            grid.SetState(5, 5, 1);
            grid.SetState(5, 6, 1);
            grid.SetState(5, 7, 1);
            grid.SetState(5, 8, 1);
            grid.SetState(5, 9, 1);
            grid.SetState(4, 9, 1);

            return grid;
        }

        static private Rule createRule()
        {
            Rule rule = new Rule(NeighborhoodTypes.Moore);

            // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
            Transition transition1 = new Transition(DEAD, x => 
            { 
                return (x.NeighborCount(LIVE) == 2 && x.LocalCell == LIVE); 
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
            Rule rule = new Rule(NeighborhoodTypes.Moore);

            // Any live cell with fewer than two live neighbours dies, as if caused by under-population.
            Transition transition1 = new Transition(DEAD, x =>
            {
                return (x.NeighborCount(LIVE) < 2 && x.LocalCell == LIVE);
            });

            // Any live cell with two or three live neighbours lives on to the next generation.
            Transition transition2 = new Transition(LIVE, x =>
            {
                return ((x.NeighborCount(LIVE) == 2 || x.NeighborCount(LIVE) == 3) && x.LocalCell == LIVE);
            });

            // Any live cell with more than three live neighbours dies, as if by overcrowding.
            Transition transition3 = new Transition(DEAD, x =>
            {
                return (x.NeighborCount(LIVE) > 3 && x.LocalCell == LIVE);
            });

            // Any live cell with more than three live neighbours dies, as if by overcrowding.
            Transition transition4 = new Transition(LIVE, x =>
            {
                return (x.NeighborCount(LIVE) == 3 && x.LocalCell == DEAD);
            });

            

            rule.AddTransition(transition1);
            rule.AddTransition(transition2);
            rule.AddTransition(transition3);
            rule.AddTransition(transition4);

            return rule;
        }
    }
}
