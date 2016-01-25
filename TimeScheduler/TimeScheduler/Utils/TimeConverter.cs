using System;

namespace TimeScheduler.Utils
{
    public class TimeConverter
    {
        private TimeConverter() { }

        private static String _schema = "{0:00}:{1:00} {2:00}";

        public static String fromSeconds(long seconds)
        {
            long hours = seconds / 3600;
            long minutes = (seconds - hours * 3600) / 60;
            seconds = seconds - hours * 3600 - minutes * 60;
            return String.Format(_schema, hours, minutes, seconds);
        }

        public static long toSeconds(int hours)
        {
            return hours * 3600;
        }

        public static long toSeconds(long minutes)
        {
            return minutes * 60;
        }

        public static long toSeconds(int hours, long minutes)
        {
            return  toSeconds(hours) + toSeconds(minutes);
        }
    }
}
