#define TEST_THIS_LIBRARY
using System;
using System.Collections;
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
            Debug.Log("This is lib test");
        }

#endif
        private EmptyHandler() { }

    }
}