using CellularAutomaton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CellularGUI
{
    /// <summary>
    ///     Repeats next generation process
    /// </summary>
    public class AutomatonDispatcher
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        DispatcherTimer timer = new DispatcherTimer();

        private int speed;

        public int Speed
        {
            get { return speed; }
            set 
            { 
                speed = value;
                timer.Interval = new TimeSpan(0, 0, 0, 0, speed);
            }
        }

        private Automaton automaton;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        
        /// <summary>
        ///     speed in milliseconds
        /// </summary>
        /// <param name="automaton"></param>
        /// <param name="speed"></param>
        public AutomatonDispatcher(Automaton automaton, int speed)
        {
            Speed = speed;
            this.automaton = automaton;

            timer.Tick += new EventHandler(dispatcherTimer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, speed);
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            automaton.NextGeneration();
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
