using System;
using System.Linq;

namespace TimeScheduler.Stores
{
    class TimerStatesStore
    {
        static TimerStatesStore()
        {
            CurrentState = StateUndefiend;
        }

        public const int StateStop = 0;
        public const int StateWork = 1;
        public const int StatePause = 2;
        public const int StateResume = 3;
        public const int StateUndefiend = 5;

        private static int _state = StateStop;
        private static event Action OnStateChanged; 

        public static int CurrentState
        {
            get { return _state;  }
            set
            {
                switch (value)
                {
                    case StateStop:
                    case StateWork:
                    case StatePause:
                    case StateResume:
                        _state = value;
                        break;
                    default:
                        _state = StateUndefiend;
                        break;
                }
                OnStateChanged?.Invoke();
            }
        }

        public static void AddStateChangedEvent(Action stateChanged)
        {
            OnStateChanged += stateChanged;
        }

        public static bool IsStopped()
        {
            return (CurrentState == StateStop) ;
        }

        public static bool IsStarted()
        {
            return (CurrentState == StateWork);
        }

        public static bool IsPaused()
        {
            return (CurrentState == StatePause);
        }

        public static bool IsResumed()
        {
            return (CurrentState == StateResume);
        }

        public static bool IsUndefined()
        {
            return (CurrentState == StateUndefiend);
        }

        public static bool IsAnother(params int[] states)
        {
            return states.Any(state => CurrentState == state);
        }

        public static bool IsAnotherThan(params int[] states)
        {
            return states.All(state => CurrentState != state);
        }
    }
}
