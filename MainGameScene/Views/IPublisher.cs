using UnityEngine;
using System.Collections.Generic;
using MainGameScene.Messages;

namespace MainGameScene.View
{
    public interface IPublisher<TValue>
    {
        public TValue Publish();
    }

    public interface IPublisher<TValue, TContext>
    {
        public IEnumerable<OutEvent<TValue, TContext>> Publish();
    }

    public abstract class PublishableActor<TValue> : MonoBehaviour, IPublisher<TValue>
    {
        public abstract TValue Publish();
    }
}