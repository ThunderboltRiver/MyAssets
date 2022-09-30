using MainGameScene.Model;
using MainGameScene.View;
using UnityEngine;

namespace MainGameScene.Presenter
{
    public class MoveDirectionPresenter : MonoBehaviour
    {
        [SerializeField] SlopeJudger _slopeJuder;
        [SerializeField] Walker _walker;
        [SerializeField] MoveController _moveController;

        /// <summary>
        /// 通信の処理
        /// </summary>
        void FixedUpdate()
        {
            if (_slopeJuder.IsWalkableSlope())
            {
                _walker.MovingDirection(_moveController.moveDirection, _slopeJuder.normalVector);
            }
        }

    }
}