using System;
using System.Windows;
using System.Windows.Threading;

namespace TimeScheduler.Services
{
    public class TimerService
    {

        private int _tickInterval = 1;

        public delegate void OnTickEventHandler(long time, long tick);
        private event OnTickEventHandler OnTickEvent;

        public delegate void OnRestartEventHandler();
        private event OnRestartEventHandler OnRestartEvent;

        private event Action OnLowTimeLeft;

        private readonly DispatcherTimer _timer;
        private long _currentTime;
                private long _timelapse;
        private int _lowLimmit;
        public bool IsActive { get; set; }

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

        public void Add(OnTickEventHandler onTickMethod, OnRestartEventHandler onRestartMethod, Action onLowTimeLeft)
        {
            OnTickEvent += onTickMethod;
            OnRestartEvent += onRestartMethod;
            OnLowTimeLeft += onLowTimeLeft;
        }


        public void Set(long timelapse, int tickInterval)
        {
            _timelapse = timelapse;
            _tickInterval = tickInterval;
            _timer.Interval = new TimeSpan(0, 0, _tickInterval);
        }

        public void Start()
        {
            IsActive = true;
            _lowLimmit = (int) Math.Ceiling(_timelapse * 0.1);
            _lowLimmit = _lowLimmit%10 == 0 ? _lowLimmit : _lowLimmit + (10 - _lowLimmit%10);
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            IsActive = false;
        }

        private void OnRestart()
        {
            _currentTime = 0;
        }

        private void OnTick(object sender, object e)
        {
            _currentTime += _tickInterval;
            if (_currentTime >= _timelapse + _tickInterval) OnEnd();
            else if (Math.Abs(_currentTime - _timelapse) == _lowLimmit) OnLowTimeLeft?.Invoke();
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
