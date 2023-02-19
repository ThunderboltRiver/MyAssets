using MainGameScene.Messages;
using MainGameScene.Model;
using MainGameScene.View;
using UnityEngine;

namespace MainGameScene.Presenter
{
    public class MoveDirectionPresenter : MonoBehaviour
    {
        //[SerializeField] SlopeJudger _slopeJuder;
        [SerializeField] InputableActor<Vector2, int> _fpsActor;
        [SerializeField] MoveController _moveController;
        [SerializeField] PublishableActor<Vector2> _strokableArea;

        void Update()
        {
            //Debug.Log(_strokableArea.Publish());
            _fpsActor.AcceptInput(new Event<Vector2, int>(_strokableArea.Publish(), (int)FirstPersonActor.AcceptableKeys.RotateCamera));
        }

        /// <summary>
        /// 通信の処理
        /// </summary>
        void FixedUpdate()
        {
            _fpsActor.AcceptInput(new Event<Vector2, int>(_moveController.moveDirection, (int)FirstPersonActor.AcceptableKeys.MoveRigidbody));
        }

    }
}