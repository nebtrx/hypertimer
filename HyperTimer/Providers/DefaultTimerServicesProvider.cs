using System.Timers.Services;

namespace System.Timers.Providers
{
    public class DefaultTimerServicesProvider : BaseTimerServicesProvider
    {
        public override ITimerServices GetTimerServices()
        {
            return new DefaultTimerServices();
        }
    }
}