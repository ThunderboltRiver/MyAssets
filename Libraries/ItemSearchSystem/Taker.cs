using System.Collections.Generic;
using UnityEngine;

namespace ItemSearchSystem
{
    public class Taker
    {
        private Stack<ITakable> _takableStack = new();
        public int _takableStackMask;

        public bool TryPushTakable(GameObject gameObject)
        {
            return gameObject.TryGetComponent(out ITakable takable) && TryPushTakable(takable);
        }

        public bool TryPushTakable(ITakable takable)
        {
            if (_takableStack.Count < _takableStackMask)
            {
                _takableStack.Push(takable);
                return true;
            }
            return false;
        }

        public void Take()
        {
            if (_takableStack.TryPop(out ITakable takable))
            {
                takable.OnTaken();
            }
        }

        public bool HasTakableObject(GameObject gameObject)
        {
            return gameObject.TryGetComponent(out ITakable takable) && HasTakableObject(takable);
        }

        public bool HasTakableObject(ITakable takable)
        {
            return _takableStack.Contains(takable);
        }
    }
}