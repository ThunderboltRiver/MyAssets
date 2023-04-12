using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace StateMachine
{
    public class StateMachine<TOwner>
    {
        private Dictionary<int, Dictionary<State, State>> _transitions = new();

        public TOwner Owner { get; }
        public State CurrentState { get; private set; }
        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }
        public void Start<T>() where T : State, new()
        {

        }
        public void AddTransition<T1, T2>(int eventKey) where T1 : State, new() where T2 : State, new()
        {
        }
        public void DispatchEvent(int key)
        {

        }

    }
}