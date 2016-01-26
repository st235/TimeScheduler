namespace TimeScheduler.Stores
{
    public class BaseStatesStore
    {
        private BaseStatesStore() { }

        public static bool IsWork { get; set; } = false;

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
