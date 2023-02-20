#define TEST_THIS_LIBRARY
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace InputableActor
{
    /// <summary>
    /// 入力値を受け取るジェネリックなハンドラクラス
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class InputHandler<TValue>
    {
        /// <summary>
        /// ActorのUpdate関数内で呼び出される
        /// </summary>
        /// <param name="value"></param>
        internal void Update(TValue value)
        {
            OnUpdate(value);
        }
        /// <summary>
        /// ActorのLateUpdate関数内で呼び出される
        /// </summary>
        /// <param name="value"></param>
        internal void LateUpdate(TValue value)
        {
            OnLateUpdate(value);
        }
        /// <summary>
        /// ActorのFixedUpdate内で呼び出される
        /// </summary>
        /// <param name="value"></param>

        internal void FixedUpdate(TValue value)
        {
            OnFixedUpdate(value);
        }

        protected virtual void OnUpdate(TValue value) { }
        protected virtual void OnLateUpdate(TValue value) { }
        protected virtual void OnFixedUpdate(TValue value) { }

        public virtual void LoadSetting<TSetting>(string settingKey, TSetting setting) { }

    }
    /// <summary>
    /// 何もしないジェネリックなハンドラクラス(シングルトン)
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    internal sealed class EmptyHandler<TValue> : InputHandler<TValue>
    {
        internal static EmptyHandler<TValue> Singletone = new();
#if TEST_THIS_LIBRARY
        protected override void OnUpdate(TValue value)
        {
            Debug.Log("This is lib test at OnUpdate");
        }
        protected override void OnLateUpdate(TValue value)
        {
            Debug.Log("This is lib test at OnLateUpdate");
        }

#endif
        private EmptyHandler() { }

    }

    public abstract class InputHandlerRefactor<TValue>
    {
        /// <summary>
        /// 入力値のキュー
        /// </summary>
        protected ConcurrentQueue<TValue> inputQueue = new();

        /// <summary>
        /// inputQueueの最大容量
        /// </summary>
        [SerializeField] protected int inputQueueMask = 0;

        public void AcceptInput(TValue value)
        {
            if (inputQueue.Count < inputQueueMask)
            {
                inputQueue.Enqueue(InputFilter(value));
            }
        }

        protected virtual TValue InputFilter(TValue value)
        {
            return value;
        }

        /// <summary>
        /// ActorのUpdate関数内で呼び出される
        /// </summary>
        internal void Update()
        {
            OnUpdate();
        }
        /// <summary>
        /// ActorのLateUpdate関数内で呼び出される
        /// </summary>
        internal void LateUpdate()
        {
            OnLateUpdate();
        }
        /// <summary>
        /// ActorのFixedUpdate内で呼び出される
        /// </summary>

        internal void FixedUpdate()
        {
            OnFixedUpdate();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnFixedUpdate() { }

        public virtual void LoadSetting<TSetting>(string settingKey, TSetting setting) { }

        public InputHandlerRefactor()
        {
            OnAwake();
            Debug.Log("OnAwake");
        }

    }
    internal sealed class EmptyHandlerRefactor<TValue> : InputHandlerRefactor<TValue>
    {
        internal static EmptyHandlerRefactor<TValue> Singletone = new();
#if TEST_THIS_LIBRARY
        protected override void OnUpdate()
        {
            Debug.Log("This is lib test at OnUpdate");
        }
        protected override void OnLateUpdate()
        {
            Debug.Log("This is lib test at OnLateUpdate");
        }
#endif
        private EmptyHandlerRefactor() { }


    }
}