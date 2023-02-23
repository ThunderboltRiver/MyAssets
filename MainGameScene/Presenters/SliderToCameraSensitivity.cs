using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using MainGameScene.Model;

namespace MainGameScene.Presenter
{

    public class SliderToCameraSensitivity : MonoBehaviour
    {
        [SerializeField] Slider _slider;
        [SerializeField] FirstPersonActor _actor;
        public void Start()
        {
            _slider.OnValueChangedAsObservable()
            .Subscribe(
                value => _actor.SettingCameraSensitivity((float)value)
            )
            .AddTo(this);


        }
    }

}