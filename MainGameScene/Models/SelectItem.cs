using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MainGameScene.Model
{
    public class SelectItem : MonoBehaviour
    {
        public readonly ReactiveProperty<GameObject> itemObject = new ReactiveProperty<GameObject>(null);

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                itemObject.Value = other.gameObject;
                return;
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other == itemObject.Value)
            {
                InititemObject();
            }
        }

        public void InititemObject()
        {
            itemObject.Value = null;
        }

    }

}