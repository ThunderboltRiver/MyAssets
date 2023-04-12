using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace StateMachine
{
    public class StateMachine<TOwner>
    {
        private Dictionary<State, Dictionary<int, State>> _transitions = new();

        public abstract class State
        {
            internal StateMachine<TOwner> stateMachine;
            public State() { }
            protected internal virtual void OnEnter() { }
            public static bool operator ==(State a, State b)
            {
                return a.GetType() == b.GetType();
            }
            public static bool operator !=(State a, State b)
            {
                return a.GetType() != b.GetType();
            }
            public override bool Equals(object obj)
            {
                return GetType() == obj.GetType();
            }

            public override int GetHashCode()
            {
                return GetType().GetHashCode();
            }

        }
        private class EmptyState : State { }
        public TOwner Owner { get; }
        public State CurrentState { get; private set; } = new EmptyState();
        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }

        public T RegisterOrGetState<T>() where T : State, new()
        {
            var wantStates = _transitions.Keys.Where(x => x is T);
            if (wantStates.Count() == 0)
            {
                var state = new T();
                _transitions.Add(state, new Dictionary<int, State>());
                return state;
            }
            return wantStates.First() as T;
        }

        public void Start<T>() where T : State, new()
        {
            if (CurrentState is not EmptyState)
            {
                throw new InvalidOperationException("State is already started.");
            }
            CurrentState = RegisterOrGetState<T>();
        }

        public void AddTransition<T1, T2>(int eventKey) where T1 : State, new() where T2 : State, new()
        {
        }
        public void DispatchEvent(int key)
        {

        }

    }
}