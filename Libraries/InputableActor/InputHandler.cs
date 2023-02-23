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

        public virtual void LoadSetting<TSettingKey, TSetting>(TSettingKey settingKey, TSetting setting) { }
        public virtual TSetting GetSetting<TSettingKey, TSetting>(TSettingKey settingKey) where TSetting : new()
        {
            return new TSetting();
        }
        public InputHandler()
        {
            OnAwake();
            Debug.Log("OnAwake");
        }

    }
    internal sealed class EmptyHandler<TValue> : InputHandler<TValue>
    {
        internal static EmptyHandler<TValue> Singletone = new();
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
        private EmptyHandler() { }


    }
}