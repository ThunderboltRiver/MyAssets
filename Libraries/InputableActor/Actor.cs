using System.Collections.Concurrent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InputableActor
{
    public class Actor<TValue> : MonoBehaviour
    {
        private ConcurrentQueue<TValue> eventQueue = new();
        private TValue _inputedValue;
        public void AcceptInput(TValue value)
        {
            eventQueue.Enqueue(InputFilter(value));
        }
        protected virtual TValue InputFilter(TValue value)
        {
            return value;
        }

        private InputHandler<TValue> _inputHandler = EmptyHandler<TValue>.Singletone;
        public void Start()
        {
            OnStart();
        }
        public void Update()
        {
            eventQueue.TryDequeue(out _inputedValue);
            _inputHandler.Update(_inputedValue);
            OnUpdate();

        }

        public void FixedUpdate()
        {
            _inputHandler.FixedUpdate(_inputedValue);
            OnFixedUpdate();
        }

        public void LateUpdate()
        {
            _inputHandler.LateUpdate(_inputedValue);
            OnLateUpdate();
        }
        public void LoadSetting<TSetting>(string settingKey, TSetting setting)
        {
            _inputHandler.LoadSetting(settingKey, setting);
        }

        protected void ChangeHandler(InputHandler<TValue> inputHandler)
        {
            _inputHandler = inputHandler;
        }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }
    }



    /// <summary>
    /// 入力値を受け取り,それに応じてUnityのゲームサイクルを回すActor
    /// </summary>
    public class Actor<TKey, TValue> : MonoBehaviour
    {
        private ConcurrentQueue<Event<TKey, TValue>> eventQueue = new();
        private TKey _inputedKey;
        private TValue _inputedValue;
        private Dictionary<TKey, InputHandler<TValue>> _inputHandlers = new();

        /// <summary>
        /// 入力値を受け付けるメソッド
        /// </summary>
        /// <param name="key">オブジェクトが持つ特定の機能にアクセスするためのキー</param>
        /// <param name="value">入力値</param>
        public void AcceptInput(TKey key, TValue value)
        {
            TKey _key;
            TValue _value;
            (_key, _value) = InputFilter(key, value);
            eventQueue.Enqueue(new Event<TKey, TValue>(_key, _value));
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
            InputHandler<TValue> _result;
            if (_inputHandlers.TryGetValue(key: key, value: out _result))
            {
                return _result;
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
            InputHandler<TValue> _result;
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
            //yield return StartCoroutine(EventCorutine(eventQueue));
        }
        public void Update()
        {
            // SycleEventQueue((inputedEvent) => GetInputHandler(inputedEvent.Key).Update(inputedEvent.Value));
            EventCorutine(eventQueue).MoveNext();
            GetInputHandler(_inputedKey).Update(_inputedValue);
            OnUpdate();

        }

        public void FixedUpdate()
        {
            // SycleEventQueue((inputedEvent) => GetInputHandler(inputedEvent.Key).FixedUpdate(inputedEvent.Value));
            EventCorutine(eventQueue).MoveNext();
            GetInputHandler(_inputedKey).FixedUpdate(_inputedValue);
            eventQueue.Clear();
            OnFixedUpdate();
        }

        public void LateUpdate()
        {
            // SycleEventQueue((inputedEvent) => GetInputHandler(inputedEvent.Key).LateUpdate(inputedEvent.Value));
            Debug.Log($"Key:{_inputedKey} ||| Value:{_inputedValue}");
            GetInputHandler(_inputedKey).LateUpdate(_inputedValue);
            OnLateUpdate();
        }

        protected void Destroy()
        {
            _inputHandlers.Clear();
        }

        // private void SycleEventQueue(Action<Event<TKey, TValue>> onSycleEvent)
        // {
        //     Event<TKey, TValue> eventCash;
        //     int i = 0;
        //     while (i < eventQueue.Count)
        //     {
        //         eventQueue.TryPeek(out eventCash);
        //         onSycleEvent(eventCash);
        //         i++;
        //     }
        // }
        private IEnumerator EventCorutine(ConcurrentQueue<Event<TKey, TValue>> queue)
        {
            Event<TKey, TValue> inputedEvent;
            while (queue.TryDequeue(out inputedEvent))
            {
                (_inputedKey, _inputedValue) = (inputedEvent.Key, inputedEvent.Value);
                yield return null;
            }

        }


        public void LoadSetting<TSetting>(TKey key, string settingKey, TSetting setting)
        {
            GetInputHandler(_inputedKey).LoadSetting(settingKey, setting);
        }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }

        ~Actor()
        {
            Destroy();
            Debug.Log("CalledDestroy");
        }
    }
}
