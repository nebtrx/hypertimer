using System.Timers.Resolvers;

namespace System.Timers.Utilities
{
    public static class DateTimeHelper
    {
        public static DateTime PreciseCurrentLocalTime
        {
            get
            {
                return DateTime.FromFileTime(ResolutionContext.Current.PrecisionFileTimeResolver.GetValue());
            }
        }

    }
}
