using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace ItemSearchSystem
{
    public class Register
    {
        public GameObject Owner { get; }

        public IObservable<RegisteredInfo> ObserveItemAdd => _itemAddSubject;
        private readonly Subject<RegisteredInfo> _itemAddSubject = new();

        private readonly Dictionary<IRegistable, int> _inventry = new() { };

        public Register()
        {
            Owner = new GameObject();
        }
        public Register(GameObject owner)
        {
            Owner = owner;
        }
        public bool TryRegist(IRegistable target)
        {
            _inventry.TryGetValue(target, out int itemNum);
            if (itemNum < target.MaxRegistalbe)
            {
                _inventry[target] = itemNum + 1;
                target.OnRegist(Owner);
                _itemAddSubject.OnNext(new RegisteredInfo(target, 1));
                return true;
            }
            return false;
        }

        public bool TryRegist(object obj)
        {
            return (obj is IRegistable registable) && TryRegist(registable);
        }

        public bool TryRegist(GameObject gameObject)
        {
            return gameObject.TryGetComponent(out IRegistable registable) && TryRegist(registable);
        }

        public IEnumerable<(IRegistable, int)> GetAllRegistered()
        {
            return _inventry.Select(pair => (pair.Key, pair.Value)).ToArray();
        }
    }

    public readonly struct RegisteredInfo
    {
        public readonly IRegistable item;
        public readonly int itemNum;

        public RegisteredInfo(IRegistable item, int itemNum)
        {
            this.item = item;
            this.itemNum = itemNum;
        }
    }
}