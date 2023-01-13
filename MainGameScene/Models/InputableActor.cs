using System.Collections.Generic;
using UnityEngine;

namespace MainGameScene.Model
{
    public readonly struct InputEvent<TValue, TKey>
    {
        public TValue Value { get; init; }
        public TKey Key { get; init; }

        public InputEvent(TValue value, TKey key)
        {
            Value = value;
            Key = key;
        }
    }
    public class InputableActor<TValue, TKey> : IInputable<InputEvent<TValue, TKey>>
    {
        private Dictionary<TKey, IInputHandler<TValue>> _inputHandlers;
        public void AcceptInput(InputEvent<TValue, TKey> inputEvent)
        {
            if (_inputHandlers.ContainsKey(inputEvent.Key))
            {
                _inputHandlers[inputEvent.Key].HandleInput(inputEvent.Value);
            }
        }

        protected void AddInputHandler(TKey key, IInputHandler<TValue> inputHandler)
        {
            if (_inputHandlers.ContainsKey(key))
            {
                _inputHandlers.Add(key, inputHandler);
                return;
            }
            Debug.Log("This Key has already been asigned.");
        }
    }
}