using System.Timers.Services;

namespace System.Timers.Providers
{
    public interface ITimerServicesProvider
    {
        ITimerServices GetTimerServices();
    }
}
