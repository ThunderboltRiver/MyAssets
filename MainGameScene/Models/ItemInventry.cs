using System;
using UnityEngine;
using UniRx;
using General.Singletons;

namespace MainGameScene.Model
{
    public class ItemInventry : MonoSingleton<ItemInventry>
    {
        [SerializeField] int maxItemNum;
        public IReadOnlyReactiveCollection<Instance> itemInventry => _itemInventry;
        private readonly ReactiveCollection<Instance> _itemInventry = new ReactiveCollection<Instance>();

        public void AddItemtoInventry(Instance itemInstance)
        {
            if (_itemInventry.Count >= maxItemNum)
            {
                Debug.Log("これ以上は持てない");
                return;
            }
            _itemInventry.Add(itemInstance);
        }
    }

}

