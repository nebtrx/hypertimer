using System.Timers.Providers;

namespace System.Timers
{
    public class HyperTimerConfiguration
    {
        public HyperTimerConfiguration()
        {
            TimerServicesProvider = new DefaultTimerServicesProvider();
        }

        public ITimerServicesProvider TimerServicesProvider { get; set; }
        
        public HyperTimerConfiguration UseTimerServicesProvider(ITimerServicesProvider timerServicesProvider)
        {
            TimerServicesProvider = timerServicesProvider;
            return this;
        }

        public HyperTimerConfiguration UseTimerServicesProvider(Func<ITimerServicesProvider> providerFunc)
        {
            TimerServicesProvider = providerFunc();
            return this;
        }
    }
}