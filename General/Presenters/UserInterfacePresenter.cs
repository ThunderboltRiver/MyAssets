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
            _keySender.IsPointerUpInAreaNotTimeOut
            .Subscribe(pointerevent =>
            {
                userInterfaceChanger.ChangeCanvas(_keySender.CanvasName);
            }
            )
            .AddTo(this);

            _keySender.IsPointerUpAfterTimeOut
            .Subscribe(pointerevent =>
            {
                Debug.Log("Charge Shot!");
            }
            ).AddTo(this);
        }
    }
}