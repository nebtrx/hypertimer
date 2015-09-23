namespace System.Timers
{
    public interface IValueResolver<T>
    {
        T GetValue();
    }
}
