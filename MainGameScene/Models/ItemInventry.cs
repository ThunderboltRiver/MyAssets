using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace MainGameScene.Model
{
    public class ItemInventry : MonoBehaviour
    {
        public ReactiveCollection<Instance> itemInventry = new ReactiveCollection<Instance>();
    }

}

