using System.Collections.Concurrent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InputableActor
{
    public class ActorRefactor<TValue> : MonoBehaviour
    {

        private InputHandlerRefactor<TValue> _inputHandler = EmptyHandlerRefactor<TValue>.Singletone;
        public void Start()
        {
            OnStart();
        }
        public void Update()
        {
            OnUpdate();
            _inputHandler.Update();
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
            _inputHandler.FixedUpdate();
        }

        public void LateUpdate()
        {
            OnLateUpdate();
            _inputHandler.LateUpdate();
        }
        public void LoadSetting<TSetting>(string settingKey, TSetting setting)
        {
            _inputHandler.LoadSetting(settingKey, setting);
        }

        protected void ChangeHandler(InputHandlerRefactor<TValue> inputHandler)
        {
            _inputHandler = inputHandler;
        }
        public void AcceptInput(TValue value)
        {
            _inputHandler.AcceptInput(value);
        }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }
    }



    /// <summary>
    /// 入力値を受け取り,それに応じてUnityのゲームサイクルを回すActorRefactor
    /// </summary>
    public class ActorRefactor<TKey, TValue> : MonoBehaviour
    {
        private Dictionary<TKey, InputHandlerRefactor<TValue>> _inputHandlers = new();

        /// <summary>
        /// 入力値を受け付けるメソッド
        /// </summary>
        /// <param name="key">オブジェクトが持つ特定の機能にアクセスするためのキー</param>
        /// <param name="value">入力値</param>
        public void AcceptInput(TKey key, TValue value)
        {
            GetInputHandler(key).AcceptInput(value);
        }

        /// <summary>
        /// キーに対応するハンドラにアクセスするメソッド.登録されていないキーに対しては何もしないハンドラを返す.
        /// </summary>
        /// <param name="key">このオブジェクトが持つ特定の機能(ハンドラ)にアクセスするためのキー</param>
        /// <returns></returns>
        protected InputHandlerRefactor<TValue> GetInputHandler(TKey key)
        {
            InputHandlerRefactor<TValue> _result;
            if (_inputHandlers.TryGetValue(key: key, value: out _result))
            {
                return _result;
            }
            return EmptyHandlerRefactor<TValue>.Singletone;
        }

        /// <summary>
        /// ハンドラの登録を行うメソッド
        /// </summary>
        /// <param name="key">登録するハンドラへのアクセスキー</param>
        /// <param name="inputHandler">登録するハンドラ</param>
        protected void AddInputHandler(TKey key, InputHandlerRefactor<TValue> inputHandler)
        {
            InputHandlerRefactor<TValue> _result;
            if (_inputHandlers.TryGetValue(key: key, value: out _result))
            {
                Debug.Log("This key has been arleady assigned");
                Debug.Log($"Key:{key} || Handler:{_result}");
                return;
            }
            _inputHandlers[key] = inputHandler;
        }
        public void Start()
        {
            Destroy();
            OnStart();
        }
        public void Update()
        {
            OnUpdate();
            foreach (InputHandlerRefactor<TValue> inputHandler in _inputHandlers.Values)
            {
                inputHandler.Update();
            }
        }

        public void FixedUpdate()
        {
            OnFixedUpdate();
            foreach (InputHandlerRefactor<TValue> inputHandler in _inputHandlers.Values)
            {
                inputHandler.FixedUpdate();
            }

        }

        public void LateUpdate()
        {
            OnLateUpdate();
            foreach (InputHandlerRefactor<TValue> inputHandler in _inputHandlers.Values)
            {
                inputHandler.LateUpdate();
            }

        }

        protected void Destroy()
        {
            _inputHandlers.Clear();
        }

        public void LoadSetting<TSetting>(TKey key, string settingKey, TSetting setting)
        {
            GetInputHandler(key).LoadSetting(settingKey, setting);
        }
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }

        ~ActorRefactor()
        {
            OnAwake();
        }
    }
}
