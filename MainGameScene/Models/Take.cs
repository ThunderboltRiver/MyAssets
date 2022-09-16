using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MainGameScene.Model
{
    public class Take : MonoBehaviour
    {
        [SerializeField] ItemInventry _inventry;
        [SerializeField] int maxitem;
        [SerializeField] Transform player;
        public void AddItemtoInventry(Instance item)
        {
            if (_inventry.getCount() >= maxitem)
            {
                Debug.Log("これ以上は持てない");
                return;
            }
            _inventry.itemInventry.Add(item);
        }

        public void SetItemAsChild(GameObject itemObject)
        {
            itemObject.transform.SetParent(player, false);
            itemObject.SetActive(false);
        }
    }
}
