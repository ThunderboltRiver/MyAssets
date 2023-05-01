using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachine
{
    /// <summary>
    /// 状態遷移を管理する有限状態機械
    /// </summary>
    /// <typeparam name="TOwner">StateMachineの所有者の型</typeparam>
    public class FiniteStateMachine<TOwner>
    {
        /// <summary>
        /// StateMachineの状態を表すクラス。継承して使う。
        /// </summary>
        /// <remarks>
        /// StateMachineの状態を表すクラスはこのクラスを継承して作成する。
        /// 継承したクラスは、OnEnter()、OnUpdate()、OnLateUpdate()、OnFixedUpdate()、OnExit()のいずれかをオーバーライドして使う。
        /// これらのメソッドは、Stateが遷移したときに呼び出される。
        /// </remarks>
        public abstract class State
        {
            /// <summary>
            /// Stateの所有者のインスタンス。
            /// </summary>
            protected internal FiniteStateMachine<TOwner> stateMachine;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            protected internal State() { }

            /// <summary>
            /// このStateに遷移したときに呼び出される。
            /// </summary>
            protected internal virtual void OnEnter() { }

            /// <summary>
            /// stateMachineがUpdateされるときに呼び出される。
            /// </summary>
            protected internal virtual void OnUpdate() { }

            /// <summary>
            /// stateMachineがLateUpdateされるときに呼び出される。
            /// </summary>
            protected internal virtual void OnLateUpdate() { }

            /// <summary>
            /// stateMachineがFixedUpdateされるときに呼び出される。
            /// </summary>
            protected internal virtual void OnFixedUpdate() { }

            /// <summary>
            /// このStateから抜けるときに呼び出される。
            /// </summary>
            protected internal virtual void OnExit() { }

            /// <summary>
            /// 別のStateと等価かどうかを返す。TypeとStateMachineの参照が等しいときに等価とみなす。
            /// </summary>
            public static bool operator ==(State a, State b)
            {
                return a.GetType() == b.GetType() && a.stateMachine == b.stateMachine;
            }

            public static bool operator !=(State a, State b)
            {
                return a.GetType() != b.GetType() || a.stateMachine != b.stateMachine;
            }

            /// <summary>
            /// 他のオブジェクトと等価かどうかを返す。TypeとStateMachineの参照が等しいときに等価とみなす。
            /// </summary>
            public override bool Equals(object obj)
            {
                return GetType() == obj.GetType() && stateMachine == (obj as State).stateMachine;
            }

            /// <summary>
            /// HashCodeを返す。TypeとStateMachineの参照のハッシュコードをXORしたものを返す。
            /// </summary>
            public override int GetHashCode()
            {
                return GetType().GetHashCode() ^ stateMachine.GetHashCode();
            }
        }

        /// <summary>
        /// StateMachineの所有者。
        /// </summary>
        public TOwner Owner { get; }

        /// <summary>
        /// StateMachineの開始前の空の状態を表すState。
        /// </summary>
        private class EmptyState : State { }
        private readonly EmptyState _emptyState = new();

        /// <summary>
        /// 現在のState。
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// StateMachineの遷移テーブル。キーは現在の状態、値は現在の状態から遷移する状態の辞書。
        /// </summary>
        private readonly Dictionary<State, Dictionary<int, State>> _transitions = new();

        /// <summary>
        /// StateMachineを初期化する。初期状態は空の状態。
        /// </summary>
        public FiniteStateMachine(TOwner owner)
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
        /// Stateのインスタンスを登録する。登録に成功したらtrueを返す。すでに登録されている場合はfalseを返す。
        /// </summary>
        /// <param name="state"></param>
        /// <typeparam name="T"></typeparam>
        public bool RegisterState<T>(T state) where T : State
        {
            RegisterValidation(state);
            state.stateMachine = this;
            return _transitions.TryAdd(state, new Dictionary<int, State>());
        }

        /// <summary>
        /// 現在の状態を強制的に変更する.内部で変更前のStateのOnExit()と変更後のStateのOnEnter()を呼び出す。
        /// </summary>
        /// <typeparam name="T">変更後の具象Stateの型</typeparam>
        private void ChangeState<T>() where T : State, new()
        {
            CurrentState.OnExit();
            CurrentState = _emptyState is T ? _emptyState : RegisterOrGetState<T>();
            CurrentState.OnEnter();
        }

        ///<summary>
        /// 現在の状態を強制的に変更する
        /// </summary>
        /// <param name="state">変更後の具象Stateのインスタンス</param>
        private void ChangeState(State state)
        {
            CurrentState.OnExit();
            CurrentState = state;
            CurrentState.OnEnter();
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
            ChangeState<T>();
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
        /// <exception cref="InvalidOperationException">まだ開始されていない場合</exception>
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
            ChangeState<EmptyState>();
        }

        /// <summary>
        /// FromStateからToStateへの遷移をeventKeyで登録する。FromStateからの同じeventKeyでの遷移がすでに登録されている場合は上書きされずに例外を投げる。
        /// </summary>
        /// <typeparam name="TFrom">FromStateの具象Stateの型</typeparam>
        /// <typeparam name="TTo">ToStateの具象Stateの型</typeparam>
        /// <param name="eventKey">遷移を登録するためのキー</param>
        public void AddTransition<TFrom, TTo>(int eventKey) where TFrom : State, new() where TTo : State, new()
        {
            if (IsActive)
            {
                throw new InvalidOperationException("State is already started. You can't add transition after starting.");
            }
            var from = RegisterOrGetState<TFrom>();
            var to = RegisterOrGetState<TTo>();
            if (!_transitions[from].TryAdd(eventKey, to))
            {
                throw new InvalidOperationException("Transition is already registered.");
            }
        }

        private void RegisterValidation(State state)
        {
            if (state.stateMachine != null && state.stateMachine != this)
            {
                throw new InvalidOperationException("State is already registered in other StateMachine.");
            }
        }

        public void AddTransition(State from, State to, int eventKey)
        {
            if (IsActive)
            {
                throw new InvalidOperationException("State is already started. You can't add transition after starting.");
            }
            RegisterState(from);
            RegisterState(to);
            if (!_transitions[from].TryAdd(eventKey, to))
            {
                throw new InvalidOperationException("Transition is already registered.");
            }
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
        /// stateMachineにeventKeyを送信する。CurrentStateに対する遷移が登録されている場合は遷移の処理を行う。
        /// </summary>
        /// <param name="eventKey">遷移を発生させるキー</param>
        /// <exception cref="InvalidOperationException">まだ開始されていない場合</exception>
        public void DispatchEvent(int eventKey)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("State is not started yet.");
            }
            if (_transitions[CurrentState].TryGetValue(eventKey, out var nextState))
            {
                ChangeState(nextState);
            }
        }

    }
}