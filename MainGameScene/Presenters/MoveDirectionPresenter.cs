using MainGameScene.Messages;
using MainGameScene.Model;
using MainGameScene.View;
using UnityEngine;
using InputableActor;

namespace MainGameScene.Presenter
{
    public class MoveDirectionPresenter : MonoBehaviour
    {
        //[SerializeField] SlopeJudger _slopeJuder;
        [SerializeField] FirstPersonActor _fpsActor;
        [SerializeField] MoveController _moveController;
        [SerializeField] PublishableActor<Vector2> _strokableArea;
        //[SerializeField] WalkableActor _walker;

        void Update()
        {
            //Debug.Log(_strokableArea.Publish());
            _fpsActor.AcceptInput((int)FirstPersonActor.AcceptableKeys.MoveRigidbody, _moveController.moveDirection);
            _fpsActor.AcceptInput((int)FirstPersonActor.AcceptableKeys.RotateCamera, _strokableArea.Publish());
            //_walker.AcceptInput(_moveController.moveDirection);
        }

    }
}