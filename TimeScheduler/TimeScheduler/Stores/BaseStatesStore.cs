namespace TimeScheduler.Stores
{
    public class BaseStatesStore
    {

        private static bool _isWork = false;

        private BaseStatesStore() { }

        public static bool IsWork 
        {
            get { return _isWork; }
            set { _isWork = value; }
        }
        
        public static void Init(bool isWork)
        {
            _isWork = isWork;
        }

        public static void Revert()
        {
            _isWork = !_isWork;
        }
    }
}
