using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    public class StateMachine<TOwner>
    {
        private readonly Dictionary<State, Dictionary<int, State>> _transitions = new();

        public abstract class State
        {
            protected internal StateMachine<TOwner> stateMachine;
            public State() { }
            protected internal virtual void OnEnter() { }
            protected internal virtual void OnUpdate() { }
            protected internal virtual void OnLateUpdate() { }
            protected internal virtual void OnFixedUpdate() { }
            protected internal virtual void OnExit() { }
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
        private readonly EmptyState _emptyState = new();
        public TOwner Owner { get; }
        public State CurrentState { get; private set; }
        public StateMachine(TOwner owner)
        {
            Owner = owner;
            CurrentState = _emptyState;
        }
        /// <summary>
        /// StateMachineがすでに開始されているかどうかを返す。
        /// </summary>
        /// <returns>開始されているかどうか</returns>
        public bool IsActive => CurrentState is not EmptyState;

        /// <summary>
        /// 既に登録されているStateならそれを返す。なければ新しく登録してから返す。
        /// </summary>
        /// <typeparam name="T">登録したい具象Stateの型</typeparam>
        /// <returns>登録された具象Stateのインスタンス</returns>
        private T RegisterOrGetState<T>() where T : State, new()
        {
            var state = new T { stateMachine = this };
            return _transitions.TryAdd(state, new Dictionary<int, State>()) ? state : _transitions.Keys.Where(x => x == state).First() as T;
        }

        /// <summary>
        /// StateMachineを開始する。すでに開始されている場合は例外を投げる。
        /// </summary>
        /// <typeparam name="T">開始する具象Stateの型</typeparam>
        /// <exception cref="InvalidOperationException">すでに開始されている場合</exception>
        /// <exception cref="ArgumentException">引数の型が具象Stateでない場合</exception>
        public void Start<T>() where T : State, new()
        {
            if (IsActive)
            {
                throw new InvalidOperationException("State is already started.");
            }
            CurrentState = RegisterOrGetState<T>();
            CurrentState.OnEnter();
        }

        /// <summary>
        /// FromStateからToStateへの遷移がeventKeyで登録されているかどうかを返す。
        /// </summary>
        /// <typeparam name="TFrom">FromStateの具象Stateの型</typeparam>
        /// <typeparam name="TTo">ToStateの具象Stateの型</typeparam>
        /// <param name="eventKey">遷移を登録するためのキー</param>
        /// <returns>登録されているかどうか</returns>
        public bool HasTransition<TFrom, TTo>(int eventKey) where TFrom : State, new() where TTo : State, new()
        {
            if (_transitions.Keys.Count(state => state is TFrom) == 0)
            {
                return false;
            }
            var from = _transitions.Keys.Where(state => state is TFrom).First();
            return _transitions[from][eventKey] is TTo;
        }

        /// <summary>
        /// StateMachineを更新する。 まだ開始されていない場合は例外を投げる。
        /// MonobehaviourのUpdate()などで呼び出す。
        /// </summary>
        /// <exception cref="InvalidOperationException">まだ開始されていない場合</exception>
        public void Update()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("State is not started yet.");
            }
            CurrentState.OnUpdate();
        }

        /// <summary>
        /// StateMachineを固定フレーム数ごとに更新する。 まだ開始されていない場合は例外を投げる。
        /// MonobehaviourのFixedUpdate()などで呼び出す。
        /// </summary>
        /// <exception cref="InvalidOperationException">まだ開始されていない場合</exception>
        public void FixedUpdate()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("State is not started yet.");
            }
            CurrentState.OnFixedUpdate();
        }

        /// <summary>
        /// StateMachineを更新した後に呼び出す。 まだ開始されていない場合は例外を投げる。
        /// MonoBehaviourのLateUpdate()などで呼び出す。
        /// </summary>
        public void LateUpdate()
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("State is not started yet.");
            }
            CurrentState.OnLateUpdate();
        }

        /// <summary>
        /// StateMachineを停止する。すでに停止されている場合は何もしない。
        /// </summary>
        public void Stop()
        {
            if (!IsActive)
            {
                return;
            }
            CurrentState.OnExit();
            CurrentState = _emptyState;
        }

        /// <summary>
        /// FromStateからToStateへの遷移をeventKeyで登録する。FromStateからの同じeventKeyでの遷移がすでに登録されている場合は上書きされずに例外を投げる。
        /// </summary>
        /// <typeparam name="TFrom">FromStateの具象Stateの型</typeparam>
        /// <typeparam name="TTo">ToStateの具象Stateの型</typeparam>
        /// <param name="eventKey">遷移を登録するためのキー</param>
        public void AddTransition<TFrom, TTo>(int eventKey) where TFrom : State, new() where TTo : State, new()
        {
            var from = RegisterOrGetState<TFrom>();
            var to = RegisterOrGetState<TTo>();
            if (!_transitions[from].TryAdd(eventKey, to))
            {
                throw new InvalidOperationException("Transition is already registered.");
            }
        }

        /// <summary>
        /// stateMachineにeventKeyを送信する。CurrentStateに対する遷移が登録されている場合は遷移の処理を行う。
        /// </summary>
        /// <param name="eventKey">遷移を登録するためのキー</param>
        /// <exception cref="InvalidOperationException">まだ開始されていない場合</exception>
        public void DispatchEvent(int eventKey)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("State is not started yet.");
            }
            if (!_transitions[CurrentState].TryGetValue(eventKey, out var nextState))
            {
                return;
            }
            CurrentState.OnExit();
            CurrentState = nextState;
            CurrentState.OnEnter();
        }

    }
}