using TimeScheduler.Utils;

namespace TimeScheduler.Stores
{
    public class TimerStore
    {
        private static long WORK_TIMELAPSE, REST_TIMELPASE;
        private static int TICK_INTERVAL = 1;

        public static long Timelapse
        {
            get
            {
                if (BaseStatesStore.IsWork) return WORK_TIMELAPSE;
                return REST_TIMELPASE;
            }
            set 
            {
                if (BaseStatesStore.IsWork) WORK_TIMELAPSE = value;
                else REST_TIMELPASE = value;
            }
        }

        public static int TickInterval
        {
            get { return TICK_INTERVAL; }
        }

        private TimerStore() { }

        public static void Set(long workTimelapse, long restTimelapse)
        {
            WORK_TIMELAPSE = TimeConverter.toSeconds(workTimelapse);
            REST_TIMELPASE = TimeConverter.toSeconds(restTimelapse);
        }
    }
}
