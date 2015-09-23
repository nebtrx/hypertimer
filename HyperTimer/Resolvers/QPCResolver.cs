using System.Timers.Utilities;

namespace System.Timers.Resolvers
{
    public class QPCResolver : ITimeResolver
    {
        private static readonly Lazy<long> _tickFrequency = new Lazy<long>(() =>
        {
            long frequency = 0;
            WinAPIHelper.QueryPerformanceFrequency(out frequency);
            return frequency;
        });

        public static long TickFrequency { get { return _tickFrequency.Value; } }

        public long GetValue()
        {
            long counter = 0L;
            WinAPIHelper.QueryPerformanceCounter(out counter);
            return (long)((double)counter * TickFrequency);
        }
    }
}