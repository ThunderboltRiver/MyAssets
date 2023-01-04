using System;
namespace MainGameScene.Model
{
    public interface ISubscriber<T>
    {
        public void Subscribe(T message);
    }

    public class Subscriber<T> : ISubscriber<T>
    {
        private Action<T> _onSubscribe;
        public void Subscribe(T message)
        {
            _onSubscribe(message);
        }

        public void Subscribe(Action<T> onSubscribe)
        {
            _onSubscribe = onSubscribe;
        }
    }
}