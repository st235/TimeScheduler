namespace TimeScheduler.Stores
{
    public static class BaseStatesStore
    {
        public static bool IsWork { get; set; }

        public static void Init(bool isWork)
        {
            IsWork = isWork;
        }

        public static void Revert()
        {
            IsWork = !IsWork;
        }
    }
}
