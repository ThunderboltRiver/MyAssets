using System;
using System.Linq;
using JetBrains.Annotations;
using ObservableCollections;
using UnityEngine;
using UnityEngine.UIElements;

namespace ItemSearchSystem
{
    public class Taker
    {
        public IObservableCollection<ITakable> WaitingTakablesAsObservableCollection => _takableStack;
        private readonly ObservableStack<ITakable> _takableStack = new();
        public Transform Transform { get; }
        public float MaxDistance { get; }
        private int _takableStackMask;
        public int TakableStackMask
        {
            get => _takableStackMask;
            set => _takableStackMask = value > 0 ? value : 0;
        }

        public int TakableCount => _takableStack.Count;

        public Taker(Transform transform, float maxDistance)
        {
            Transform = transform;
            MaxDistance = maxDistance;
        }

        public Taker(GameObject takerObject, float maxDistance)
        {
            Transform = takerObject.transform;
            MaxDistance = maxDistance;
        }

        public Taker()
        {
            Transform = new GameObject().transform;
            MaxDistance = 1.0f;
        }

        public bool TryPushTakable(GameObject gameObject)
        {
            return gameObject.TryGetComponent(out ITakable takable) && TryPushTakable(takable);
        }

        public bool TryPushTakable(ITakable takable)
        {
            if (_takableStack.Count < TakableStackMask && !_takableStack.Contains(takable))
            {
                _takableStack.Push(takable);
                return true;
            }
            return false;
        }

        public bool Take(Vector3 takeDirection, out object obj)
        {
            if (Physics.Raycast(Transform.position, takeDirection.normalized, out RaycastHit hitInfo, MathF.Min(MaxDistance, takeDirection.magnitude))
                && hitInfo.collider.gameObject.TryGetComponent(out ITakable takable)
                )
            {
                takable.OnTaken(hitInfo.normal);
                obj = takable;
                return true;
            }
            obj = null;
            return false;

        }
        public bool Take(out object obj)
        {
            return Take(Vector3.zero, out obj);
        }

        public bool HasTakableObject(GameObject gameObject)
        {
            return gameObject.TryGetComponent(out ITakable takable) && HasTakableObject(takable);
        }

        public bool HasTakableObject(ITakable takable)
        {
            return _takableStack.Contains(takable);
        }

        public void ClearTakable()
        {
            _takableStack.Clear();
        }
    }
}