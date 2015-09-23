namespace System.Timers.Resolvers
{
    public class ResolutionContext : IResolutionContext
    {
        private static readonly Lazy<IResolutionContext> _syncResolutionContext = new Lazy<IResolutionContext>(() => new ResolutionContext());

        private readonly Lazy<ITimeResolver> _qpcResolver;

        private readonly Lazy<ITimeResolver> _dateTimeResolver;

        private readonly Lazy<ITimeResolver> _precisionDateTimeResolver;

        public ResolutionContext()
        {
            _qpcResolver =  new Lazy<ITimeResolver>(() => new QPCResolver());
            _dateTimeResolver = new Lazy<ITimeResolver>(() => new DateTimeResolver());
            _precisionDateTimeResolver = new Lazy<ITimeResolver>(() => new PrecisionFileTimeResolver());
        }

        public ITimeResolver QPCResolver { get { return _qpcResolver.Value; } }

        public ITimeResolver TimeResolver { get { return _dateTimeResolver.Value; } }

        public ITimeResolver PrecisionFileTimeResolver { get { return _precisionDateTimeResolver.Value; } }

        public static IResolutionContext Current { get { return _syncResolutionContext.Value; } }
    }
}
