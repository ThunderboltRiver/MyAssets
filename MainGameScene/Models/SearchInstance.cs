using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGameScene.Model
{
    public class SearchInstance : MonoBehaviour
    {
        [SerializeField] InstanceDataBase allInstanceData;

        public Instance FromGameObject(GameObject go)
        {
            return allInstanceData.InstanceList.Find(insrance => insrance.InstanceName == go.name);
        }
    }
}