using UnityEngine;
using UniRx;
using General.Models;
using General.Views;

namespace General.Presenter
{
    public class ScenePresenter : MonoBehaviour
    {
        [SerializeField] private PressableUI _button;
        [SerializeField] private SceneNavigater _navigater;
        void Start()
        {
            _button.isPressed
            .Where(value => value)
            .Subscribe(value =>
            {
                _navigater.LoadScene();
            }).AddTo(this);
        }
    }
}
