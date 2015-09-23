using System.Timers.Utilities;

namespace System.Timers.Resolvers
{
    public class PrecisionFileTimeResolver : ITimeResolver
    {
        public long GetValue()
        {
            long fileTime = 0L;
            WinAPIHelper.GetSystemTimePreciseAsFileTime(out fileTime);
            return DateTime.FromFileTime(fileTime).Ticks;
        }
    }
}