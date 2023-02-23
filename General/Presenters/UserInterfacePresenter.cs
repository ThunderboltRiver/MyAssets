using UnityEngine;
using UniRx;
using General.Models;
using General.Views;

namespace General.Presenter
{
    public class UserInterfacePresenter : MonoBehaviour
    {
        [SerializeField] CanvasKeySender _keySender;
        void Start()
        {
            UserInterfaceChanger userInterfaceChanger = UserInterfaceChanger.I;
            _keySender.GetEventData()
            .Subscribe(pointerevent =>
            {
                userInterfaceChanger.ChangeCanvas(_keySender.CanvasName);
            }
            )
            .AddTo(this);
        }
    }
}