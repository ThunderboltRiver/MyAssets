using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainGameScene.Model
{
    public class SearchInstance : MonoBehaviour
    {
        [SerializeField] InstanceDataBase allInstanceData;

        public Instance FromInstanceName(string InstanceName)
        {
            return allInstanceData.InstanceList.Find(insrance => insrance.InstanceName == InstanceName);
        }
    }
}