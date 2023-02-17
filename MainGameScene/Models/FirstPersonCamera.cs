#define DEBUG
using System;
using UnityEngine;

namespace MainGameScene.Model
{
    public class FirstPersonCamera : IInputHandler<Vector2>
    {
        private Transform _playerTransform;
        private Transform _cameraTransform;

        public float Sensitivity { get; set; }
        public void HandleInput(Vector2 input)
        {
            RotateCamera(input);
        }

        private void RotateCamera(Vector2 input)
        {
            Vector3 inputTransformRight = input.x * _playerTransform.right;
            Vector3 inputTransformUp = input.y * _cameraTransform.up;
            float rotateAngle_x = Mathf.Rad2Deg * Mathf.Atan(inputTransformRight.sqrMagnitude / _cameraTransform.forward.sqrMagnitude) * Time.deltaTime * Sensitivity;
            float rotateAngle_y = Mathf.Rad2Deg * Mathf.Atan(inputTransformUp.sqrMagnitude / _cameraTransform.forward.sqrMagnitude) * Time.deltaTime * Sensitivity;
            Quaternion rot_x = Quaternion.AngleAxis(rotateAngle_x, input.x * _playerTransform.up);
            Quaternion rot_y = Quaternion.AngleAxis(rotateAngle_y, input.y * -_cameraTransform.right);

            // 先にカメラを回転させないと,Playerの回転後にこの関数を抜けたあとにPlayerの子オブジェクトとしてのカメラが意図しない方向に回転してしまう
            _cameraTransform.rotation = rot_y * _cameraTransform.rotation;
            _playerTransform.rotation = rot_x * _playerTransform.rotation;
        }
        /// <summary>
        /// 一人称視点のカメラの作成
        /// </summary>
        /// <param name="cameraTransform">回転するカメラ</param>
        /// <param name="playerTransform">カメラが追従するプレイヤー</param>
        public FirstPersonCamera(Transform cameraTransform, Transform playerTransform, float sensitivity)
        {
            _cameraTransform = cameraTransform;
            _playerTransform = playerTransform;
            Sensitivity = sensitivity;
            //視点用のカメラをplayerの子オブジェクトに指定する.
            _cameraTransform.SetParent(_playerTransform);
        }
    }

}