using System.Collections;
using System.Collections.Generic;
using MainGameScene.Messages;
using UnityEngine;
using System.Linq;


namespace MainGameScene.View
{
    public class PublishersComponent<TValue, TContext> : IPublisher<TValue, TContext>
    {
        Dictionary<IPublisher<TValue>, TContext> _contexts = new Dictionary<IPublisher<TValue>, TContext>() { };

        public IEnumerable<OutEvent<TValue, TContext>> Publish()
        {
            List<OutEvent<TValue, TContext>> messages = new List<OutEvent<TValue, TContext>>();
            foreach (IPublisher<TValue> publisher in _contexts.Keys)
            {
                messages.Add(CreateOutEvent(publisher));
            }
            return messages;
        }

        private OutEvent<TValue, TContext> CreateOutEvent(IPublisher<TValue> publisher)
        {
            return new OutEvent<TValue, TContext>(publisher.Publish(), _contexts[publisher]);
        }

        public void AddPublisher(IPublisher<TValue> publisher, TContext context)
        {
            if (!_contexts.ContainsKey(publisher))
            {
                _contexts.Add(publisher, context);
                return;
            }
            Debug.Log("This publisher has already been asigned.");

        }

        public void RemovePublisher(IPublisher<TValue> publisher)
        {
            _contexts.Remove(publisher);
        }

    }

}
