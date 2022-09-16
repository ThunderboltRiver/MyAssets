
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using MainGameScene.Model;
using General.View;

namespace MainGameScene.Presenter
{
    public class TakePresenter : MonoBehaviour
    {
        //Models
        [SerializeField] Take _take;
        [SerializeField] SelectItem _selectitem;

        [SerializeField] SearchInstance _searchinstance;

        //Views
        [SerializeField] PressableUI _hand;

        void Start()
        {
            /// <summary>
            /// アイテムを選択しているならHandを有効にする
            /// </summary>
            _selectitem.itemObject
            .Subscribe(value =>
            {
                bool isHandActive = value != null;
                _hand.gameObject.SetActive(isHandActive);
            }).AddTo(this);

            /// <summary>
            /// Handが押されたらアイテムをインベントリに追加することを試みる
            /// </summary>
            _hand.isPressed
            .Where(value => _selectitem.itemObject.Value != null && value)
            .Subscribe(value =>
            {
                GameObject itemObject = _selectitem.itemObject.Value;
                Instance iteminstance = _searchinstance.FromGameObject(itemObject);
                _take.AddItemtoInventry(iteminstance, itemObject);
                _selectitem.InititemObject();
                _hand.isPressed.Value = false;
            }).AddTo(this);

            /// <summary>
            /// アイテムがインベントリに正常に追加されたら選択中のアイテムを初期化する
            /// </summary>
            // _take.isSucceed
            // .Where(value => value)
            // .Subscribe(value =>
            // {
            //     _selectitem.InititemObject();
            //     Debug.Log("init!!!!");
            // }).AddTo(this);
        }
    }


}