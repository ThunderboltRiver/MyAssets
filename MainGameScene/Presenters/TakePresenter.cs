
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
            _selectitem.itemName
            .Subscribe(itemname =>
            {
                bool isHandActive = itemname.Length > 0;
                _hand.gameObject.SetActive(isHandActive);
            }).AddTo(this);


            _hand.isPressed
            .Where(value => _selectitem.itemName.Value.Length > 0 && value)
            .Subscribe(value =>
            {
                Instance iteminstance = _searchinstance.FromInstanceName(_selectitem.itemName.Value);
                _take.AddItemtoInventry(iteminstance);
            }).AddTo(this);
        }
    }


}