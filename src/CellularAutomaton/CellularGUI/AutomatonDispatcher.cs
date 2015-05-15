using CellularAutomaton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CellularGUI
{
    /// <summary>
    ///     Repeats next generation process
    /// </summary>
    public class AutomatonDispatcher : INotifyPropertyChanged
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
                OnPropertyChanged("Speed");
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
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
