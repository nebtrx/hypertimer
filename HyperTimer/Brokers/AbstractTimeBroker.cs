using System.ComponentModel;
using System.Threading;
using System.Timers.Brokers;
using System.Timers.Resolvers;

namespace System.Timers.Brokers
{
    public abstract class AbstractTimeBroker : BaseBroker<long>, ITimeBroker
    {
        protected readonly ITimeResolver _timeResolver;
        protected long _throttleTimeTicks;

        protected AbstractTimeBroker(ITimeResolver timeResolver)
        {
            _timeResolver = timeResolver;
        }

        public override IValueResolver<long> ValueResolver
        {
            get { return _timeResolver; }
        }

        protected override long BrokerLoopHandler(CancelEventArgs args)
        {
            return GetTimeTicks();
        }

        protected abstract long GetTimeTicks();

        protected long GetThrottleTimeTicks()
        {
            return Interlocked.Exchange(ref _throttleTimeTicks, 0);
        }

        protected void AddThrottleTimeTicks(long aheadTicks)
        {
            Interlocked.Add(ref _throttleTimeTicks, aheadTicks);
        }

        public void Throttle(TimeSpan throttleTime)
        {
            AddThrottleTimeTicks(throttleTime.Ticks);
        }

        public void Throttle(double throttleTimeInMiliseconds)
        {
            AddThrottleTimeTicks(TimeSpan.FromMilliseconds(throttleTimeInMiliseconds).Ticks);
        }
    }
}