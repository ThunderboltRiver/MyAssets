using MainGameScene.Model;
using MainGameScene.View;
using UnityEngine;

namespace MainGameScene.Presenter
{
    public class PlayerControllPresenter : MonoBehaviour
    {
        [SerializeField] PlayerController _playerController;
        [SerializeField] ControlHandle _controllHandle;
        void Update()
        {
            Vector2 direction = _controllHandle.Direction;
            _playerController.MoveToDirection(direction);
        }

    }
}