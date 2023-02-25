using System;
using ItemSearchSystem;
using UnityEngine;

namespace MainGameScene.Model
{

    [Serializable]
    [CreateAssetMenu(fileName = "DebugItem", menuName = "CreateDebugItem")]
    public class DebugItem : ScriptableObject, ISearchable, ISelectable, IUseable
    {
        public GameObject prefab;
        public void OnSearch()
        {
            Debug.Log("Searched DebugItem");
        }

        public void OnSelect()
        {
            Debug.Log("Selected This DebugItem");
        }

        public void Use()
        {
            Debug.Log("Called UseMethod in This DebugItem");
        }
    }


}