using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] Item.Type itemtype;
    public void pickup(){
        Debug.Log(itemtype);
    }
}
