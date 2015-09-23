using System.Timers.Brokers;
using System.Timers.Resolvers;

namespace System.Timers.Utilities
{
    public static class Extensions
    {
        public static long AsQPCTicks(this DateTime dateTime)
        {
            long currentQPCTicks = ResolutionContext.Current.QPCResolver.GetValue();
            DateTime currentTime = DateTime.FromFileTime(ResolutionContext.Current.PrecisionFileTimeResolver.GetValue());
            TimeSpan timeOffset = dateTime > currentTime ? dateTime.Subtract(currentTime) : currentTime.Subtract(dateTime);

            return currentQPCTicks + timeOffset.AsQPCTicks();
        }

        public static long AsQPCTicks(this TimeSpan timeSpan)
        {
            return (long)(timeSpan.Ticks / QPCBroker.TickFrequency);
        }

        public static TimeSpan FromQPCTicksToTimeSpan(this long qpcTicks)
        {
            return TimeSpan.FromTicks((long)(qpcTicks * QPCBroker.TickFrequency));
        }

        public static void TimesExecute(this int counter, Action action)
        {
            for (int i = 0; i < counter; i++)
            {
                action();
            }
        }

        public static void TimesExecute(this long counter, Action action)
        {
            for (int i = 0; i < counter; i++)
            {
                action();
            }
        }
    }
}
