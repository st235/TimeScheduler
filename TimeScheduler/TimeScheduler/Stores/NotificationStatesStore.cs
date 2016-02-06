namespace TimeScheduler.Stores
{
    public static class NotificationStatesStore
    {

        public static bool IsEnabled { get; set; } = true;

        public static void Revert()
        {
            IsEnabled = !IsEnabled;
        }
    }
}
