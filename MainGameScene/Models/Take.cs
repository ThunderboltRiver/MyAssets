using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MainGameScene.Model
{
    public class Take : MonoBehaviour
    {
        [SerializeField] ItemInventry _inventry;
        [SerializeField] int maxItem;
        [SerializeField] Transform player;

        public readonly ReactiveProperty<bool> isSucceed = new ReactiveProperty<bool>(false);
        public void AddItemtoInventry(Instance itemInstance, GameObject itemObject)
        {
            if (_inventry.itemInventry.Count >= maxItem)
            {
                Debug.Log("これ以上は持てない");
                isSucceed.Value = false;
                return;
            }
            itemObject.SetActive(false);
            itemObject.transform.SetParent(player, false);
            _inventry.itemInventry.Add(itemInstance);
            Debug.Log("追加完了");
            Debug.Log(itemObject);
            isSucceed.Value = true;
        }
    }
}
