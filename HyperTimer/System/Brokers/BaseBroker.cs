using System.ComponentModel;

namespace System.Timers
{
    public abstract class BaseBroker<T> : IBroker<T>
    {
        #region Fields

        private readonly BackgroundWorker _worker = new BackgroundWorker();

        #endregion

        #region Constructors

        protected BaseBroker()
        {
            _worker.DoWork += Worker_DoWork;
            _worker.WorkerSupportsCancellation = true;
        }

        #endregion

        #region Public Members

        public abstract IValueResolver<T> ValueResolver { get; }

        /// <summary>
        /// Starts, or resumes, measuring elapsed time for an interval.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public virtual void Start()
        {
            if (this.IsRunning)
                return;
            _worker.RunWorkerAsync();
            IsRunning = true;
        }

        public event EventHandler<T> Pushed;

        protected abstract T BrokerLoopHandler(CancelEventArgs args);

        public virtual void Stop()
        {
            if (!this.IsRunning)
                return;
            _worker.CancelAsync();
        }

        public bool IsRunning { get; private set; }

        #endregion

        #region Private/Protected Members

        protected virtual void OnPushed(T pushed)
        {
            EventHandler<T> handler = Pushed;
            if (handler != null) handler(this, pushed);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs args)
        {
            while (!_worker.CancellationPending)
            {
                OnPushed(BrokerLoopHandler(args));
            }
            args.Cancel = true;
        }

        #endregion
    }
}
