using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <summary>
        /// 既に登録されているStateならそれを返す。なければ新しく登録してから返す。
        /// </summary>
        /// <typeparam name="T">登録したい具象Stateの型</typeparam>
        /// <returns>登録された具象Stateのインスタンス</returns>
        private T RegisterOrGetState<T>() where T : State, new()
        {
            var state = new T();
            var wantStates = _transitions.Keys.Where(x => x == state);
            return _transitions.TryAdd(state, new Dictionary<int, State>()) ? state : wantStates.First() as T;
        }

        /// <summary>
        /// StateMachineを開始する。すでに開始されている場合は例外を投げる。
        /// </summary>
        /// <typeparam name="T">開始する具象Stateの型</typeparam>
        /// <exception cref="InvalidOperationException">すでに開始されている場合</exception>
        /// <exception cref="ArgumentException">引数の型が具象Stateでない場合</exception>
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