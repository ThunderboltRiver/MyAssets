using System;
using UnityEngine;
using UniRx;

public class StartButtonPresenter : MonoBehaviour
{
    [SerializeField] private StartButtonView _startbutton;
    [SerializeField] private StartButtonModel _navigater;
    void Start()
    {
        _startbutton.isPressed
        .Subscribe(x =>
        {
            Debug.Log(x);
        }).AddTo(this);
    }

}