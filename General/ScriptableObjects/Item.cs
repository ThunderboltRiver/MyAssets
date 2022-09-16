using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateItem")]
public class Item : ScriptableObject
{
    public Type type;
    public String infomation;
    public Sprite sprite;
    public GameObject prefab;
    public enum Type
    {
        Key,
        Attack,
        Important,
    }
    // public void main(Item item)
    // {
    //     this.type = item.type;
    //     this.infomation = item.infomation;
    //     this.sprite = item.sprite;

    // }
}
