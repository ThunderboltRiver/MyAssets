using UnityEngine;
using UniRx;
using General.Model;
using General.View;

namespace General.Presenter
{
    public class ScenePresenter : MonoBehaviour
    {
        [SerializeField] private NextSceneButton _button;
        [SerializeField] private SceneNavigater _navigater;
        void Start()
        {
            _button.isPressed
            .Subscribe(isPressed =>
            {
                if (isPressed) _navigater.LoadScene();
            }).AddTo(this);
        }
    }
}
