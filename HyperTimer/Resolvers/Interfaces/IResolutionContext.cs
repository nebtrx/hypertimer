namespace System.Timers.Resolvers
{
    public interface IResolutionContext
    {
        ITimeResolver QPCResolver { get; }
        ITimeResolver TimeResolver { get; }
        ITimeResolver PrecisionFileTimeResolver { get; }
    }
}