namespace System.Timers
{
    public delegate void DetailedElapsedEventHandler(IHyperTimer hyperTimer, DetailedElapseEventArgs args);

    public class DetailedElapseEventArgs
    {
        internal DetailedElapseEventArgs(long cyclesCompleted = 0, long cyclesLeft = 0, 
            TimeSpan elapsed = default(TimeSpan),
            TimeSpan totalElapsed = default(TimeSpan))
        {
            CyclesCompleted = cyclesCompleted;
            CyclesLeft = cyclesLeft;
            Elapsed = elapsed;
            TotalElapsed = totalElapsed;
        }

        public long CyclesCompleted { get; private set; }

        public long CyclesLeft { get; private set; }

        public TimeSpan Elapsed { get; private set; }

        public double ElapsedMilliseconds
        {
            get { return TotalElapsed.TotalMilliseconds; }
        }

        public double ElapsedSeconds
        {
            get { return TotalElapsed.TotalSeconds; }
        }

        public TimeSpan TotalElapsed { get; private set; }

        public double TotalElapsedMilliseconds 
        { 
            get { return TotalElapsed.TotalMilliseconds; }
        }

        public double TotalElapsedSeconds 
        { 
            get { return TotalElapsed.TotalSeconds; }
        }

    }
}
