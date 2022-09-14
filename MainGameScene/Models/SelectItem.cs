using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MainGameScene.Model
{
    public class SelectItem : MonoBehaviour
    {
        public readonly ReactiveProperty<string> itemName = new ReactiveProperty<string>("");

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                itemName.Value = other.gameObject.name;
                return;
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.name == itemName.Value)
            {
                InititemName();
            }
        }

        public void InititemName()
        {
            itemName.Value = "";
        }

    }

}