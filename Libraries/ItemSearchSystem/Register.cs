using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemSearchSystem
{
    public class Register
    {
        private readonly Dictionary<IRegistable, int> _inventry = new() { };
        public bool TryRegist(IRegistable target)
        {
            _inventry.TryGetValue(target, out int targetNum);
            if (targetNum < target.MaxRegistalbe)
            {
                _inventry[target] = targetNum + 1;
                target.OnRegist();
                return true;
            }
            return false;
        }

        public bool TryRegist(GameObject gameObject)
        {
            return gameObject.TryGetComponent(out IRegistable registable) && TryRegist(registable);

        }

        public IEnumerable<(IRegistable, int)> GetAllRegistered()
        {
            return _inventry.Select(pair => (pair.Key, pair.Value));
        }

    }
}