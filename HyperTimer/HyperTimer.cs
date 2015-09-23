using System.Threading;
using System.Timers.Services;
using System.Timers.Utilities;

namespace System.Timers
{
    public class HyperTimer: IHyperTimer
    {
        #region Fields
        private Threading.Timer _startScheduler;
        private Threading.Timer _stopScheduler;

        private readonly ITimerServices _timerServices;
        private double _totalElapsedUntilLastCycle;
        private bool _enabled;
        private long _cyclesLeft;
        private long _cyclesCompleted;
        private bool _disposed;
        #endregion

        #region Constructors
        private HyperTimer(ITimerServices timerServices)
        {
            if (timerServices == null)
                throw new ArgumentNullException("timerServices");

            _timerServices = timerServices;
            _timerServices.Elapsed += TimerServices_ElapsedHandler;
            CyclesLeft = -1;
        }

        #endregion

        #region Private/Protected Members
        private void TimerServices_ElapsedHandler(object sender, EventArgs e)
        {
            RegisterCycleCompleted();
            TotalElapsedMillisecondsUntilLastCycle += _timerServices.IntervalProgress;
            DetailedElapseEventArgs args = new DetailedElapseEventArgs(CyclesCompleted, CyclesLeft, TimeSpan.FromMilliseconds(_timerServices.IntervalProgress), /*DateTimeHelper.PreciseCurrentLocalTime - _starTime*/ TimeSpan.FromMilliseconds(_totalElapsedUntilLastCycle));
            DetailedElapsedEventHandler handler = Elapsed;
            if (handler == null)
                return;
            handler(this, args);
        }

        private void RegisterCycleCompleted()
        {
            CyclesCompleted++;
            if (CyclesLeft != -1)
                CyclesLeft--;

            if (CyclesLeft == 0)
                Stop();
        }

        #endregion

        #region IHyperTimer Members
        public long CyclesLeft
        {
            get { return Interlocked.Read(ref _cyclesLeft); }
            private set { Interlocked.Exchange(ref _cyclesLeft, value); }
        }

        public long CyclesCompleted
        {
            get { return Interlocked.Read(ref _cyclesCompleted); }
            private set { Interlocked.Exchange(ref _cyclesCompleted, value); }
        }

        public TimeSpan IntervalProgress
        {
            get 
            {
                return TimeSpan.FromMilliseconds(IntervalProgressInMilliseconds);
            }
        }

        public double IntervalProgressInMilliseconds
        {
            get
            {
                return _timerServices.IntervalProgress;
            }
        }

        public double IntervalProgressInSeconds
        {
            get
            {
                return IntervalProgress.TotalSeconds;
            }
        }

        public TimeSpan TotalElapsed
        {
            get
            {
                return TimeSpan.FromMilliseconds(TotalElapsedMilliseconds);
            }
        }

        public double TotalElapsedMilliseconds
        {
            get
            {
                return Interlocked.CompareExchange(ref _totalElapsedUntilLastCycle, 0, 0)  + IntervalProgressInMilliseconds;
            }
        }

        public double TotalElapsedSeconds
        {
            get
            {
                return TotalElapsed.TotalSeconds;
            }
        }

        public TimeSpan TotalElapsedUntilLastCycle
        {
            get
            {
                return TimeSpan.FromMilliseconds(TotalElapsedMillisecondsUntilLastCycle);
            }
        }

        public double TotalElapsedMillisecondsUntilLastCycle
        {
            get
            {
                return Interlocked.CompareExchange(ref _totalElapsedUntilLastCycle, 0, 0);
            }
            private set
            {
                Interlocked.Exchange(ref _totalElapsedUntilLastCycle, value);
            }

        }

        public double TotalElapsedSecondsUntilLastCycle
        {
            get
            {
                return TotalElapsedUntilLastCycle.TotalSeconds;
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value)
                {
                    _timerServices.Start();
                    _totalElapsedUntilLastCycle = 0;
                }
                else
                {
                    _timerServices.Stop();
                }
                _enabled = value;
            }
        }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timerServices.Interval); }
            set { _timerServices.Interval = value.TotalMilliseconds; }
        }

        public double IntervalInMilliseconds
        {
            get { return _timerServices.Interval; }
            set { _timerServices.Interval = value; }
        }

        public double IntervalInSeconds
        {
            get { return Interval.TotalSeconds; }
            set { _timerServices.Interval = TimeSpan.FromSeconds(value).TotalMilliseconds; }
        }

        public IHyperTimer RepeatForever()
        {
            CyclesLeft = -1;
            return this;
        }

        public IHyperTimer RepeatJust(long repetitions)
        {
            if (repetitions - CyclesCompleted <= 0)
                Stop();
            else
                CyclesLeft = repetitions - CyclesCompleted;
            return this;
        }

        public IHyperTimer RepeatMore(int repetitionsIncrement)
        {
            CyclesLeft += repetitionsIncrement;
            return this;
        }

        public IHyperTimer RepeatLess(int repetitionsDecrement)
        {
            CyclesLeft -= repetitionsDecrement;
            return this;
        }

        public event DetailedElapsedEventHandler Elapsed;

        public event EventHandler Stopped;

        public IHyperTimer Start()
        {
            Enabled = true;
            return this;
        }

        public IHyperTimer StartAt(DateTime starTime)
        {
            if (!Enabled)
            {
                DateTime now = DateTimeHelper.PreciseCurrentLocalTime;

                if (starTime <= now)
                {
                    throw new ArgumentOutOfRangeException("starTime");
                }

                _startScheduler = new Threading.Timer(state => Start(), null, starTime - now, Timeout.InfiniteTimeSpan); 
            }

            return this;
        }

        public IHyperTimer StartIn(TimeSpan dueTime)
        {
            if (!Enabled)
            {
                if (dueTime < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException("dueTime");
                }

                _startScheduler = new Threading.Timer(state => Start(), null, dueTime, Timeout.InfiniteTimeSpan);
            }

            return this;
        }

        public IHyperTimer StartIn(long dueTimeInMilliseconds)
        {
            return StartIn(TimeSpan.FromMilliseconds(dueTimeInMilliseconds));
        }

        public IHyperTimer Stop()
        {
            Enabled = false;
            
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
            return this;
        }

        public IHyperTimer StopIn(int dueExecutions)
        {
            Interlocked.Exchange(ref _cyclesLeft, dueExecutions);
            return this;
        }

        public IHyperTimer StopIn(TimeSpan dueTime)
        {
            if (dueTime < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("dueTime");
            }

            _stopScheduler = new Threading.Timer(state => Stop(), null, dueTime, Timeout.InfiniteTimeSpan);

            return this;
        }

        public IHyperTimer StopIn(long dueTimeInMilliseconds)
        {
            StopIn(TimeSpan.FromMilliseconds(dueTimeInMilliseconds));
            return this;
        }

        public IHyperTimer StopAt(DateTime stopTime)
        {
            DateTime now = DateTimeHelper.PreciseCurrentLocalTime;

            if (stopTime <= now)
            {
                throw new ArgumentOutOfRangeException("stopTime");
            }

            _stopScheduler = new Threading.Timer(state => Stop(), null, stopTime - now, Timeout.InfiniteTimeSpan);

            return this;
        }

        public IHyperTimer Throttle(TimeSpan throttleTime)
        {
            _timerServices.Throttle(throttleTime);
            return this;
        }

        public IHyperTimer Throttle(double throttleTimeInMiliseconds)
        {
            _timerServices.Throttle(throttleTimeInMiliseconds);
            return this;
        }

        public IHyperTimer Delay(TimeSpan delayTime)
        {
            _timerServices.Sleep(delayTime);
            return this;
        }

        public IHyperTimer Delay(double delayTimeInMiliseconds)
        {
            _timerServices.Sleep(delayTimeInMiliseconds);
            return this;
        }

        public IHyperTimer Wait(TimeSpan timeOut)
        {
            Thread.Sleep(timeOut);
            return this;
        }

        public IHyperTimer Wait(double timeOutInMilliseconds)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(timeOutInMilliseconds));
            return this;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _startScheduler.Dispose();
                    _stopScheduler.Dispose();
                }
                Stop();
            }
            _disposed = true;
        }

        ~HyperTimer()
        {
            Dispose (false);
        }

        #endregion

        #region Static Members
        private static HyperTimerConfiguration Configuration { get; set; }

        static HyperTimer()
        {
            Configuration = new HyperTimerConfiguration();
        }

        public static HyperTimerConfiguration Configure()
        {
            return Configuration;
        }

        public static HyperTimerConfiguration Reconfigure()
        {
            return Configuration = new HyperTimerConfiguration();
        }

        private static IHyperTimer Create(DetailedElapsedEventHandler elapsedEventHandler, EventHandler stoppedEventHandler = null)
        {
            var hyperTimer = new HyperTimer(Configuration.TimerServicesProvider.GetTimerServices());
            if (elapsedEventHandler != null)
                hyperTimer.Elapsed += elapsedEventHandler;
            if (stoppedEventHandler != null) 
                hyperTimer.Stopped += stoppedEventHandler;
            return hyperTimer;
        }

        public static IHyperTimer New(TimeSpan interval, DetailedElapsedEventHandler elapsedEventHandler, EventHandler stoppedEventHandler = null)
        {
            var hyperTimer = Create(elapsedEventHandler, stoppedEventHandler);
            hyperTimer.Interval = interval;
            return hyperTimer;
        }

        public static IHyperTimer New(double intervalInMilliseconds, DetailedElapsedEventHandler elapsedEventHandler, EventHandler stoppedEventHandler = null)
        {
            var hyperTimer = Create(elapsedEventHandler, stoppedEventHandler);
            hyperTimer.IntervalInMilliseconds = intervalInMilliseconds;
            return hyperTimer;
        }

        public static IHyperTimer StartNew(TimeSpan interval, DetailedElapsedEventHandler elapsedEventHandler, EventHandler stoppedEventHandler = null)
        {
            var hyperTimer = New(interval, elapsedEventHandler, stoppedEventHandler);
            hyperTimer.Start();
            return hyperTimer;
        }

        public static IHyperTimer StartNew(double intervalInMilliseconds, DetailedElapsedEventHandler elapsedEventHandler, EventHandler stoppedEventHandler = null)
        {
            var hyperTimer = New(intervalInMilliseconds, elapsedEventHandler, stoppedEventHandler);
            hyperTimer.Start();
            return hyperTimer;
        }

        #endregion
    }
}
