namespace System.Timers.Brokers
{
    public interface ITimeIntervalBroker : IBroker<long>
    {
        void Throttle(TimeSpan throttleTime);

        void Throttle(double throttleTimeInMiliseconds);
    }
}