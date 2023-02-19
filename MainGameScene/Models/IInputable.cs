using System.Collections.Generic;
using MainGameScene.Messages;
using UnityEngine;
namespace MainGameScene.Model
{
    public interface IInputable<T>
    {
        public void AcceptInput(T input);
    }
    public abstract class InputableActor<TValue, TKey> : MonoBehaviour, IInputable<Event<TValue, TKey>>
    {
        private Dictionary<TKey, IInputHandler<TValue>> _inputHandlers = new Dictionary<TKey, IInputHandler<TValue>>() { };
        public void AcceptInput(Event<TValue, TKey> inputEvent)
        {
            if (_inputHandlers.ContainsKey(inputEvent.Key()))
            {
                _inputHandlers[inputEvent.Key()].HandleInput(inputEvent.Value());
            }
        }
        /// <summary>
        /// IInputHandlerの追加
        /// </summary>
        /// <param name="inputHandler">追加したいハンドラ</param>
        /// <param name="key">追加するハンドラにアクセスするキー</param>
        protected void AddInputHandler(IInputHandler<TValue> inputHandler, TKey key)
        {
            if (!_inputHandlers.ContainsKey(key))
            {
                _inputHandlers.Add(key, inputHandler);
                return;
            }
            Debug.Log("This Key() has already been asigned.");
        }
    }

}