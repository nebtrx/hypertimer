using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers.Brokers;
using System.Timers.Brokers;
using System.Timers.Utilities;

namespace System.Timers.Services
{
    public class QPCTimerServices: ITimerServices
    {
        private readonly ITimeBroker _timeBroker;
        private long _timeCheckPoint;
        private long _qpcInterval;
        private bool _enabled;
        private bool _disposed;

        public QPCTimerServices(ITimeBroker timeBroker)
        {
            _timeBroker = timeBroker;
            _timeBroker.Pushed += TimeBroker_PushedHandler;
        }

        private void TimeBroker_PushedHandler(object sender, long time)
        {
            if (Enabled)
            {
                var elapsed = time - _timeCheckPoint;
                if (elapsed  >= _qpcInterval)
                {
                    var pendingCounter = elapsed / _qpcInterval;

                    //if (Elapsed != null)
                    //{
                    //    pendingCounter.TimesExecute(() => Elapsed(this, null));
                    //}    

                    if (Elapsed != null)
                        Elapsed(this, null);

                    _timeCheckPoint += _qpcInterval * pendingCounter;
                }
            }
            else
            {
                _timeCheckPoint = time;
            }
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
                return _qpcInterval.FromQPCTicksToTimeSpan().TotalMilliseconds;
            }
            set
            {
                _qpcInterval = TimeSpan.FromMilliseconds(value).AsQPCTicks();
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                ThrowIfDisposed();
                if (value)
                {
                    _timeCheckPoint = _timeBroker.ValueResolver.GetValue();
                    _timeBroker.Start();
                }
                else
                {
                    _timeBroker.Stop();
                }
                _enabled = value;
            }
        }

        public event EventHandler Elapsed;

        public double IntervalProgress { get; private set; }

        public void Throttle(TimeSpan throttleTime)
        {
            _timeBroker.Throttle(throttleTime);
        }

        public void Throttle(double throttleTimeInMiliseconds)
        {
            _timeBroker.Throttle(throttleTimeInMiliseconds);
        }

        public void Sleep(TimeSpan delayTime)
        {
            throw new NotImplementedException();
        }

        public void Sleep(double delayTimeInMiliseconds)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            //if (!_disposed)
            //{
            //    if (disposing)
            //    {
            //        // TODO:
            //    }
            //}
            _disposed = true;
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }


        ~QPCTimerServices()
        {
            Dispose (false);
        } 
    }
}
