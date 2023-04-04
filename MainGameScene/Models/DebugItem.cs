using System;
using ItemSearchSystem;
using UnityEngine;

namespace MainGameScene.Model
{

    [Serializable]
    //[CreateAssetMenu(fileName = "DebugItem", menuName = "CreateDebugItem")]
    public class DebugItem : MonoBehaviour, ISearchable, ITakable, IRegistable
    {
        int IRegistable.MaxRegistalbe => 1;
        public GameObject prefab;
        public void OnSearch()
        {
            Debug.Log("Searched DebugItem");
        }
        public void OnDesearch()
        {
            Debug.Log("DeSearched DebugItem");
        }

        public void OnSelected()
        {
            Debug.Log("Selected DebugItem");
        }
        public void OnTaken(Vector3 takeDirection)
        {
            Debug.Log("Taken DebugItem");
        }
        public void OnDeselected()
        {
            Debug.Log("DeSelected DebugItem");
        }

        public void OnRegist(GameObject owner)
        {
            Debug.Log($"Registed DebugItem at {owner.name}");
        }

    }


}