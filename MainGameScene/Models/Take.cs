using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGameScene.Model
{
    public class Take : MonoBehaviour
    {
        [SerializeField] InstanceDataBase iteminventry;
        [SerializeField] int maxitem;
        [SerializeField] Transform player;
        public void AddItemtoInventry(Instance item)
        {
            List<Instance> contents = iteminventry.InstanceList;
            if (contents.Count >= maxitem)
            {
                Debug.Log("これ以上は持てない");
                return;
            }
            contents.Add(item);
        }

        public void SetItemAsChild(GameObject itemObject)
        {
            itemObject.transform.SetParent(player, false);
            itemObject.SetActive(false);
        }
    }
}
