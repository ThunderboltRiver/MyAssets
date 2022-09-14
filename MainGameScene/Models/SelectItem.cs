using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MainGameScene.Models
{
    public class SelectItem : MonoBehaviour
    {
        public readonly StringReactiveProperty itemName = new StringReactiveProperty("");

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                itemName.Value = other.gameObject.name;
                return;
            }
            itemName.Value = "";
        }

    }

}