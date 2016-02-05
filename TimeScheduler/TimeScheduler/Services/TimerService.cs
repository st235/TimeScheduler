using System;
using System.Windows.Threading;

namespace TimeScheduler.Services
{
    public class TimerService
    {

        private int _tickInterval = 1;
        private long _timelapse;

        public delegate void OnTickEventHandler(long time, long tick);
        private event OnTickEventHandler OnTickEvent;

        public delegate void OnRestartEventHandler();
        private event OnRestartEventHandler OnRestartEvent;

        private readonly DispatcherTimer _timer;
        private long _currentTime;

        public long Timelapse
        { 
            get { return _timelapse;  }
            set { _timelapse = value; }
         }

         public long CurrentTime
         {
             get { return _currentTime; }
             set { _currentTime = value; }
         }

        public TimerService()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += OnTick;
        }

        public void Add(OnTickEventHandler onTickMethod, OnRestartEventHandler onRestartMethod)
        {
            OnTickEvent += onTickMethod;
            OnRestartEvent += onRestartMethod;
        }


        public void Set(long timelapse, int tickInterval)
        {
            _timelapse = timelapse;
            _tickInterval = tickInterval;
            _timer.Interval = new TimeSpan(0, 0, _tickInterval);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnRestart()
        {
            _currentTime = 0;
        }

        private void OnTick(object sender, object e)
        {
            _currentTime += _tickInterval;
            if (_currentTime >= _timelapse + _tickInterval) OnEnd();
            OnTickEvent?.Invoke(this.Timelapse, this.CurrentTime);
        }

        private void OnEnd()
        {
            OnRestart();
            OnRestartEvent?.Invoke();
            _currentTime = 0;
        }
    }
}
