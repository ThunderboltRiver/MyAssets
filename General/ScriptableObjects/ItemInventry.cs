using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ItemInventry : MonoBehaviour
{
    [SerializeFild] InstanceDataBase instanceDataBase;
    public ReactiveCollection<Instance> itemInventry;

    void Awake()
    {
        itemInventry = new ReactiveCollection<Instance>(instanceDataBase);
    }
}

