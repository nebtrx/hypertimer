namespace System.Timers 
{
    public interface IHyperTimer: IDisposable
    {
        event DetailedElapsedEventHandler Elapsed;

        long CyclesLeft { get; }

        long CyclesCompleted { get; }

        bool Enabled { get; set; }

        TimeSpan Interval { get; set; }

        double IntervalInMilliseconds { get; set; }

        double IntervalInSeconds { get; set; }

        TimeSpan IntervalProgress { get; } 

        double IntervalProgressInMilliseconds { get; }

        double IntervalProgressInSeconds { get; }

        TimeSpan TotalElapsed { get; }

        double TotalElapsedMilliseconds { get; }

        double TotalElapsedSeconds { get; }

        TimeSpan TotalElapsedUntilLastCycle { get; }

        double TotalElapsedMillisecondsUntilLastCycle { get; }

        double TotalElapsedSecondsUntilLastCycle { get; }

        IHyperTimer RepeatForever();

        IHyperTimer RepeatJust(long repetitions);

        IHyperTimer RepeatMore(int repetitionsIncrement);

        IHyperTimer RepeatLess(int repetitionsDecrement);

        IHyperTimer Start();

        IHyperTimer StartAt(DateTime starTime);

        IHyperTimer StartIn(TimeSpan dueTime);

        IHyperTimer StartIn(long dueTimeInMilliseconds);

        IHyperTimer Stop();

        IHyperTimer StopIn(int dueExecutions);

        IHyperTimer StopIn(TimeSpan dueTime);

        IHyperTimer StopIn(long dueTimeInMilliseconds);

        IHyperTimer StopAt(DateTime stopTime);

        IHyperTimer Throttle(TimeSpan throttleTime);

        IHyperTimer Throttle(double throttleTimeInMiliseconds);

        IHyperTimer Delay(TimeSpan delayTime);

        IHyperTimer Delay(double delayTimeInMiliseconds);

        IHyperTimer Wait(TimeSpan timeOut);

        IHyperTimer Wait(double timeOutInMilliseconds);

        event EventHandler Stopped;
    }
}