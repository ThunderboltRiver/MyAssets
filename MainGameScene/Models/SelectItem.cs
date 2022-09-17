using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MainGameScene.Model
{
    public class SelectItem : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<GameObject> itemObject => _itemObject;
        private readonly ReactiveProperty<GameObject> _itemObject = new ReactiveProperty<GameObject>(null);

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                _itemObject.Value = other.gameObject;
                return;
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _itemObject.Value)
            {
                InitProperty();
            }
        }

        public void InitProperty()
        {
            _itemObject.Value = null;
        }

    }

}