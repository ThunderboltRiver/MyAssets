using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameScene.View;
using MainGameScene.Model;
using UniRx;

namespace MainGameScene.Presenter
{
    public class InventryPresenter : MonoBehaviour
    {
        //ViewList
        [SerializeField] List<InventryWindow> _inventryWindows;

        //Models
        [SerializeField] ItemInventry _inventry;


        void Start()
        {
            _inventry.itemInventry
            .ObserveCountChanged()
            .Where(count => count > 0)
            .Subscribe(count =>
            {
                for (int i = 0; i < count; i++)
                {
                    _inventryWindows[i].image.sprite = _inventry.itemInventry[i].item.sprite;
                }
            }).AddTo(this);
        }


    }
}