using System.Timers.Resolvers;
using System.Timers.Utilities;

namespace System.Timers.Brokers
{
    public class QPCBroker : AbstractTimeBroker
    {
        #region Constants

        private const long TicksPerMillisecond = 10000L;
        private const long TicksPerSecond = 10000000L;

        #endregion

        #region Static Members

        /// <summary>
        /// Gets the frequency of the timer as the number of ticks per second. This field is read-only.
        /// </summary>
        public static readonly long Frequency;
        /// <summary>
        /// Indicates whether the timer is based on a high-resolution performance counter. This field is read-only.
        /// </summary>

        internal static readonly double TickFrequency;

        public QPCBroker(ITimeResolver timeResolver)
            : base(timeResolver)
        {
        }


        #endregion

        #region Constructors

        static QPCBroker()
        {
            if (!WinAPIHelper.QueryPerformanceFrequency(out QPCBroker.Frequency))
                throw new NotSupportedException();

            TickFrequency = TicksPerSecond / (double)QPCBroker.Frequency;
        }

        #endregion

        #region Private/Protected Members

        protected override long GetTimeTicks()
        {
            return (long)((double)_timeResolver.GetValue() * QPCBroker.TickFrequency) + GetThrottleTimeTicks();
        }

        #endregion
    }
}
