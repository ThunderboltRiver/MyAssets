
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
            _selectitem.itemObject
            .Subscribe(value =>
            {
                bool isHandActive = value != null;
                _hand.gameObject.SetActive(isHandActive);
            }).AddTo(this);


            _hand.isPressed
            .Where(value => _selectitem.itemObject.Value != null && value)
            .Subscribe(value =>
            {
                GameObject itemObject = _selectitem.itemObject.Value;
                Instance iteminstance = _searchinstance.FromInstanceName(itemObject.name);
                _take.AddItemtoInventry(iteminstance);
            }).AddTo(this);
        }
    }


}