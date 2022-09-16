using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemGenerater : MonoBehaviour
{
    [SerializeField] Item item;
    public List<Vector3> positions = new List<Vector3>();
    [SerializeField] List<GameObject> itemobjects;
    [SerializeField] InstanceDataBase allInstaceData;
    void Start()
    {
        itemobjects = generateItem();
        RecordtoDataBase();
    }
    List<GameObject> generateItem()
    {
        return positions.Select(position => Instantiate(item.prefab, position, Quaternion.identity)).ToList();
    }
    void RecordtoDataBase()
    {
        GameObject itemobject = itemobjects[0];
        Instance iteminstance = Instance.CreateInstance<Instance>();
        iteminstance.InstanceName = itemobject.name;
        iteminstance.item = item;
        AssetDatabase.CreateAsset(iteminstance, $"Assets/DataSets/Instance/{itemobject.name}.asset");
        allInstaceData.InstanceList.Add(iteminstance);
    }
}
