namespace System.Timers.Services
{
    public interface ITimerServices: IDisposable
    {
        void Start();

        void Stop();

        double Interval { get; set; }

        bool Enabled { get; set; }

        event EventHandler Elapsed;

        double IntervalProgress { get; } 

        void Throttle(TimeSpan throttleTime);

        void Throttle(double throttleTimeInMiliseconds);

        void Sleep(TimeSpan delayTime);

        void Sleep(double delayTimeInMiliseconds);
    }
}