using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton
{
    public class ConsoleManager
    {
            static long timeNow;
            static long timePassed;
            static long delta;

            public static void CaptureTime()
            {
                timeNow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }

            public static long GetElapsedTime()
            {
                timePassed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                delta = timePassed - timeNow;
                return delta;
            }

            public static void PrintElapsedTime(string info="")
            {
                string encloser = "%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%\n";

                timePassed = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                delta = timePassed - timeNow;

                Console.Write(encloser);
                if(!info.Equals(""))
                    Console.Write("%\t\t\t"+info + "\n");
                Console.Write("%\t\t\tTime: " + delta  + " ms\n");
                Console.Write(encloser + "\n");
            }

            

    }
}
