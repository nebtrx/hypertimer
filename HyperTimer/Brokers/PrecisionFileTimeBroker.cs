using System.Timers.Resolvers;

namespace System.Timers.Brokers
{
    public class PrecisionFileTimeBroker : AbstractTimeBroker
    {
        #region Public Members

        public PrecisionFileTimeBroker(ITimeResolver resolver)
            : base(resolver)
        {
        }

        #endregion

        #region Private/Protected Members


        #endregion

        protected virtual long GetDateTimeTicks()
        {
            return _timeResolver.GetValue() + GetThrottleTimeTicks();
        }

        protected override long GetTimeTicks()
        {
            return GetDateTimeTicks();
        }
    }
}
