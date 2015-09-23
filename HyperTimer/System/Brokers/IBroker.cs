namespace System.Timers
{
    public interface IBroker<T>
    {
        IValueResolver<T> ValueResolver { get; }

        void Start();

        void Stop();

        bool IsRunning { get; }

        event EventHandler<T> Pushed;
    }
}