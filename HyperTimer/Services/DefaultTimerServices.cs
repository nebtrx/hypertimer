using System.Diagnostics;

namespace System.Timers.Services
{
    public class DefaultTimerServices : ITimerServices
    {
        private readonly Timer _timer;
        private readonly Stopwatch _stopwatch;
        private double _interval;
        private bool _disposed;

        public DefaultTimerServices()
        {
            _stopwatch = new Stopwatch();
            _timer = new Timer();
            _timer.Elapsed += (sender, args) => OnElapsed();
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }

        public double Interval
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                _interval = value;
                _timer.Interval = value;
            }
        }

        public bool Enabled
        {
            get 
            {
                return _timer.Enabled;
            }
            set
            {
                ThrowIfDisposed();
                _timer.Enabled = value;
                if (value)
                    _stopwatch.Start();
                else
                    _stopwatch.Stop();

            }
        }

        public event EventHandler Elapsed;

        public double IntervalProgress
        {
            get 
            {
                return _stopwatch.ElapsedMilliseconds;
            }
        }

        public void Throttle(TimeSpan throttleTime)
        {
            Throttle(throttleTime.TotalMilliseconds);
        }

        public void Throttle(double throttleTimeInMiliseconds)
        {
            if (!Enabled)
                return;

            for (int i = 0; i < Math.Floor(throttleTimeInMiliseconds / _timer.Interval); i++)
                OnElapsed();

            // interval = what's left to run
            // interval = interval - (already run + left over from dividing throttle time between interval)
            var leftTime = _interval - (_stopwatch.ElapsedMilliseconds + (throttleTimeInMiliseconds % _interval));
            if (Math.Abs(leftTime) < 0)
            {
                OnElapsed();
            }
            else if (Math.Abs(leftTime - _interval) > 0)
            {
                _timer.Interval = leftTime;
                _timer.Elapsed += ElapsedAfterThrottle;
            }
        }

        public void Sleep(TimeSpan delayTime)
        {
            Sleep(delayTime.TotalMilliseconds);
        }

        public void Sleep(double delayTimeInMiliseconds)
        {
            _timer.Interval = _stopwatch.ElapsedMilliseconds + delayTimeInMiliseconds;
            _timer.Elapsed += ElapsedAfterDelay;
        }

        protected virtual void OnElapsed()
        {
            EventHandler handler = Elapsed;
            if (handler != null) handler(this, EventArgs.Empty);
            _stopwatch.Restart();
        }

        protected virtual void ElapsedAfterThrottle(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _timer.Interval = _interval;
            _timer.Elapsed -= ElapsedAfterThrottle;
            _stopwatch.Restart();
        }

        protected virtual void ElapsedAfterDelay(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _timer.Interval = _interval;
            _timer.Elapsed -= ElapsedAfterDelay;
            _stopwatch.Restart();
        }
        
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
                    _timer.Dispose();
                }
            }
            _disposed = true;
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        ~DefaultTimerServices()
        {
            Dispose (false);
        } 
    }
}