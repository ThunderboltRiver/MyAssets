using StartScene.Model;
using StartScene.View;
using UnityEngine;
using UniRx;

namespace StartScene.Presenter
{
    public sealed class StartButtonPresenter : MonoBehaviour
    {
        [SerializeField] private StartButton _startbutton;
        [SerializeField] private MainGameNavigater _navigater;
        void Start()
        {
            _startbutton.isPressed
            .Subscribe(isPressed =>
            {
                if (isPressed) _navigater.LoadScene();
            }).AddTo(this);
        }
    }
}
