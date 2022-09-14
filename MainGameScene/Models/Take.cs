using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainGameScene.Model
{
    public class Take
    {
        [SerializeField] InstanceDataBase iteminventry;
        [SerializeField] int maxitem;
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
    }
}
