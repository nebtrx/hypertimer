using System.Timers.Services;

namespace System.Timers.Providers
{
    public abstract class BaseTimerServicesProvider : ITimerServicesProvider
    {
        public abstract ITimerServices GetTimerServices();   
    }
}