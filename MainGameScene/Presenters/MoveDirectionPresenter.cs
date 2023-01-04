using MainGameScene.Model;
using MainGameScene.View;
using UnityEngine;

namespace MainGameScene.Presenter
{
    public class MoveDirectionPresenter : MonoBehaviour
    {
        //[SerializeField] SlopeJudger _slopeJuder;
        [SerializeField] Walker _subscriber;
        [SerializeField] MoveController _moveController;

        /// <summary>
        /// 通信の処理
        /// </summary>
        void Update()
        {
            _subscriber.Subscribe(_moveController.moveDirection);
        }

    }
}