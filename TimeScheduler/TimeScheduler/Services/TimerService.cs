using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TimeScheduler.Services
{
    public class TimerService
    {

        private int TICK_INTERVAL = 1;
        private long TIMELAPSE;

        public delegate void OnTickEventHandler(long time, long tick);
        private event OnTickEventHandler onTickEvent;

        public delegate void OnRestartEventHandler();
        private event OnRestartEventHandler onRestartEvent;

        private DispatcherTimer _timer;
        private long _currentTime;

        public long Timelapse
        { 
            get { return TIMELAPSE;  }
            set { TIMELAPSE = value; }
         }

         public long CurrentTime
         {
             get { return _currentTime; }
             set { _currentTime = value; }
         }

        public TimerService()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(onTick);
        }

        public void Add(OnTickEventHandler onTickMethod, OnRestartEventHandler onRestartMethod)
        {
            onTickEvent += onTickMethod;
            onRestartEvent += onRestartMethod;
        }


        public void Set(long timelapse, int tickInterval)
        {
            TIMELAPSE = timelapse;
            TICK_INTERVAL = tickInterval;
            _timer.Interval = new TimeSpan(0, 0, TICK_INTERVAL);
        }

        public void Start()
        {
            onStart();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void onStart()
        {
            _currentTime = 0;
        }

        private void onTick(object sender, object e)
        {
            _currentTime += TICK_INTERVAL;
            if (_currentTime >= TIMELAPSE + TICK_INTERVAL) onEnd();
            onTickEvent(this.Timelapse, this.CurrentTime);
        }

        private void onEnd()
        {
            onRestartEvent();
            _currentTime = 0;
        }
    }
}
