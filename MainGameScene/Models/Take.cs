using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MainGameScene.Model
{
    public class Take : MonoBehaviour
    {
        //[SerializeField] ItemInventry _inventry;
        [SerializeField] Transform player;

        public void TakeGameObject(GameObject itemObject)
        {
            // if (_inventry.itemInventry.Count >= maxItem)
            // {
            //     Debug.Log("これ以上は持てない");
            //     return;
            // }
            itemObject.SetActive(false);
            itemObject.transform.SetParent(player, false);
            Debug.Log("追加完了");
            Debug.Log(itemObject);
        }
    }
}
