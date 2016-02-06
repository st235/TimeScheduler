using System;
using System.Linq;

namespace TimeScheduler.Stores
{
    class TimerStatesStore
    {
        static TimerStatesStore()
        {
            CurrentState = States.Undefiend;
        }

        public enum States
        {
            Stop, Work, Pause, Resume, Undefiend
        }

        private static States _state = States.Stop;
        private static event Action OnStateChanged; 

        public static States CurrentState
        {
            get { return _state;  }
            set
            {
                switch (value)
                {
                    case States.Stop:
                    case States.Work:
                    case States.Pause:
                    case States.Resume:
                        _state = value;
                        break;
                    default:
                        _state = States.Undefiend;
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
            return (CurrentState == States.Stop) ;
        }

        public static bool IsStarted()
        {
            return (CurrentState == States.Work);
        }

        public static bool IsPaused()
        {
            return (CurrentState == States.Pause);
        }

        public static bool IsResumed()
        {
            return (CurrentState == States.Resume);
        }

        public static bool IsUndefined()
        {
            return (CurrentState == States.Undefiend);
        }

        public static bool IsAnother(params States[] states)
        {
            return states.Any(state => CurrentState == state);
        }

        public static bool IsAnotherThan(params States[] states)
        {
            return states.All(state => CurrentState != state);
        }
    }
}
