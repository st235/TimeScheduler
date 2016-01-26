using TimeScheduler.Utils;

namespace TimeScheduler.Stores
{
    public class TimerStore
    {
        private static long _workTimelapse, _restTimelpase;
        private const int _tickInterval = 1;

        public static long Timelapse
        {
            get
            {
                if (BaseStatesStore.IsWork) return _workTimelapse;
                return _restTimelpase;
            }
            set 
            {
                if (BaseStatesStore.IsWork) _workTimelapse = value;
                else _restTimelpase = value;
            }
        }

        public static int TickInterval => _tickInterval;

        private TimerStore() { }

        public static void Set(long workTimelapse, long restTimelapse)
        {
            _workTimelapse = TimeConverter.ToSeconds(workTimelapse);
            _restTimelpase = TimeConverter.ToSeconds(restTimelapse);
        }
    }
}
