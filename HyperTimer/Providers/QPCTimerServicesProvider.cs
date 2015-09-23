using System.Timers.Brokers;
using System.Timers.Brokers;
using System.Timers.Resolvers;
using System.Timers.Services;

namespace System.Timers.Providers
{
    public class QPCTimerServicesProvider : BaseTimerServicesProvider
    {
        public ITimeBroker GetTimeBroker()
        {
            return new QPCBroker(ResolutionContext.Current.QPCResolver);
        }

        public override ITimerServices GetTimerServices()
        {
            return new QPCTimerServices(GetTimeBroker());
        }
    }
}