using System;
using System.Threading;
using System.Timers;
using System.Timers.Providers;

namespace HyperTimerTest
{
    class Program
    {
        private static TimeSpan _total;

        static void Main(string[] args)
        {
            HyperTimer
                .Configure()
                .UseTimerServicesProvider(new DefaultTimerServicesProvider());


            Console.WriteLine("Running hypertimer with an interval of 1 sec for over 15 cycles");
            Console.WriteLine("Then waits 5 sec(while timer keep running), throttles 3 sec," );
            Console.WriteLine("displays some info, and continues until end.\n");

            var timer = HyperTimer.StartNew(1000, timer_Elapsed, timer_Stopped)
                .StopIn(15)
                .Wait(5000);

            timer.Throttle(3000);

            //timer.Delay(1000);
            Console.WriteLine(" Progress Until last Cycle: {0}", timer.TotalElapsedUntilLastCycle);
            Console.WriteLine(" Total Elapsed Progress: {0}", timer.TotalElapsed);
            Console.ReadLine();
            

            Console.WriteLine("Total: {0}", _total);
            timer.Dispose();
        }

        static void timer_Elapsed(IHyperTimer sender, DetailedElapseEventArgs args)
        {
            Console.WriteLine("Cycles Completed:{0}, Cycles Left: {1} TE: {2} Prog: {3}", args.CyclesCompleted, args.CyclesLeft, args.TotalElapsed, args.Elapsed);
            _total += args.Elapsed;
        }

        static void timer_Stopped(object sender, EventArgs args)
        {
            Console.WriteLine("Stopped");
        }
    }
}
