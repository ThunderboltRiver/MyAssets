using System.Collections.Generic;
using UnityEngine;

namespace ItemSearchSystem
{
    public class Taker
    {
        private readonly Stack<ITakable> _takableStack = new();

        private int _takableStackMask;
        public int TakableStackMask
        {
            get => _takableStackMask;
            set => _takableStackMask = value > 0 ? value : 0;
        }

        public bool TryPushTakable(GameObject gameObject)
        {
            return gameObject.TryGetComponent(out ITakable takable) && TryPushTakable(takable);
        }

        public bool TryPushTakable(ITakable takable)
        {
            if (_takableStack.Count < TakableStackMask)
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