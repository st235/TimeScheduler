using TimeScheduler.Utils;

namespace TimeScheduler.Stores
{
    public static class TimerValuesStore
    {
        private static long _workTimelapse, _restTimelpase;
        private const int _tickInterval = 1;

        public static long Timelapse
        {
            get
            {
                return BaseStatesStore.IsWork ? _workTimelapse : _restTimelpase;
            }
            set 
            {
                if (BaseStatesStore.IsWork) _workTimelapse = value;
                else _restTimelpase = value;
            }
        }

        public static long CurrentTime { get; set; }

        public static int TickInterval => _tickInterval;

        public static void Set(long workTimelapse, long restTimelapse)
        {
            _workTimelapse = TimeConverter.ToSeconds(workTimelapse);
            _restTimelpase = TimeConverter.ToSeconds(restTimelapse);
        }
    }
}
