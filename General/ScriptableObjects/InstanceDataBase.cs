using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstanceDataBase", menuName = "CreateInstanceDataBase")]
public class InstanceDataBase : ScriptableObject
{
    public List<Instance> InstanceList = new List<Instance>();
}
