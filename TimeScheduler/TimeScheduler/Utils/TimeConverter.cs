using System;

namespace TimeScheduler.Utils
{
    public static class TimeConverter
    {

        private static readonly string Schema = "{0:00}:{1:00} {2:00}";

        public static string FromSeconds(long seconds)
        {
            long hours = seconds / 3600;
            long minutes = (seconds - hours * 3600) / 60;
            seconds = seconds - hours * 3600 - minutes * 60;
            return string.Format(Schema, hours, minutes, seconds);
        }

        public static long ToSeconds(int hours)
        {
            return hours * 3600;
        }

        public static long ToSeconds(long minutes)
        {
            return minutes * 60;
        }

        public static long ToSeconds(int hours, long minutes)
        {
            return ToSeconds(hours) + ToSeconds(minutes);
        }
    }
}
