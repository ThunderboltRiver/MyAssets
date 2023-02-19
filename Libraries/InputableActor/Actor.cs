using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InputableActor
{
    /// <summary>
    /// 入力値を受け取り,それに応じてUnityのゲームサイクルを回すActor
    /// </summary>
    public class Actor<TKey, TValue> : MonoBehaviour
    {
        private TKey _inputedKey;
        private TValue _inputedValue;
        private Dictionary<TKey, InputHandler<TValue>> _inputHandlers = new() { };

        /// <summary>
        /// 入力値を受け付けるメソッド
        /// </summary>
        /// <param name="key">オブジェクトが持つ特定の機能にアクセスするためのキー</param>
        /// <param name="value">入力値</param>
        public void AcceptInput(TKey key, TValue value)
        {
            (_inputedKey, _inputedValue) = InputFilter(key, value);
        }

        /// <summary>
        /// 入力値のフィルタリングをする仮想関数
        /// </summary>
        /// <param name="key">このオブジェクトが持つ特定の機能(ハンドラ)にアクセスするためのキー</param>
        /// <param name="value">入力値</param>
        /// <returns>フィルタリングされた入力値(デフォルトではフィルタリングしない)</returns>
        protected virtual (TKey, TValue) InputFilter(TKey key, TValue value)
        {
            return (key, value);
        }

        /// <summary>
        /// キーに対応するハンドラにアクセスするメソッド.登録されていないキーに対しては何もしないハンドラを返す.
        /// </summary>
        /// <param name="key">このオブジェクトが持つ特定の機能(ハンドラ)にアクセスするためのキー</param>
        /// <returns></returns>
        protected InputHandler<TValue> GetInputHandler(TKey key)
        {
            if (_inputHandlers.ContainsKey(_inputedKey))
            {
                return _inputHandlers[key];
            }
            return EmptyHandler<TValue>.Singletone;
        }

        /// <summary>
        /// ハンドラの登録を行うメソッド
        /// </summary>
        /// <param name="key">登録するハンドラへのアクセスキー</param>
        /// <param name="inputHandler">登録するハンドラ</param>
        protected void AddInputHandler(TKey key, InputHandler<TValue> inputHandler)
        {
            if (_inputHandlers.ContainsKey(_inputedKey))
            {
                Debug.Log("This key has been arleady assigned");
                return;
            }
            _inputHandlers[key] = inputHandler;
        }

        public void Update()
        {
            OnUpdate(_inputedKey, _inputedValue);
            GetInputHandler(_inputedKey).Update(_inputedValue);
        }

        public void FixedUpdate()
        {
            OnFixedUpdate(_inputedKey, _inputedValue);
            GetInputHandler(_inputedKey).FixedUpdate(_inputedValue);
        }

        public void LateUpdate()
        {
            OnLateUpdate(_inputedKey, _inputedValue);
            GetInputHandler(_inputedKey).LateUpdate(_inputedValue);
        }

        protected void Destroy()
        {
            _inputHandlers.Clear();
        }

        public void LoadSetting<TSetting>(TKey key, string settingKey, TSetting setting)
        {
            GetInputHandler(_inputedKey).LoadSetting(settingKey, setting);
        }
        protected virtual void OnUpdate(TKey key, TValue value) { }
        protected virtual void OnFixedUpdate(TKey key, TValue value) { }
        protected virtual void OnLateUpdate(TKey key, TValue value) { }

        ~Actor()
        {
            Destroy();
            Debug.Log("CalledDestroy");
        }
    }
}
