namespace System.Timers.Resolvers
{
    public class DateTimeResolver : ITimeResolver
    {
        public long GetValue()
        {
            return DateTime.Now.Ticks;
        }
    }
}