#define DEBUG
using System;
using UnityEngine;
using InputableActor;

namespace MainGameScene.Model
{
    [Serializable]
    public class FirstPersonCamera : InputHandler<Vector2>
    {
        [Serializable]
        private class Setting
        {
            [SerializeField] float _cameraSensitivity;
            [SerializeField] float _RotationVerticalAngleUp = -90.0f;
            [SerializeField] float _RotationVerticalAngleDown = 90.0f;
            public float CameraSensitivity => _cameraSensitivity;
            public float RotationVerticalAngleUp => _RotationVerticalAngleUp + Mathf.Epsilon;
            public float RotationVerticalAngleDown => _RotationVerticalAngleDown - Mathf.Epsilon;
        }
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Setting setting;
        private Vector2 inputedValue;

        protected override void OnAwake()
        {
            inputQueueMask = 1;
        }
        protected override void OnLateUpdate()
        {
            inputQueue.TryDequeue(out inputedValue);
            RotateCamera(inputedValue);
        }

        private void RotateCamera(Vector2 input)
        {
            Vector3 inputTransformRight = input.x * _playerTransform.right;
            Vector3 inputTransformUp = input.y * _cameraTransform.up;
            float deltaAngleHorizontal = Mathf.Rad2Deg * Mathf.Atan(inputTransformRight.sqrMagnitude / _cameraTransform.forward.sqrMagnitude) * Time.deltaTime * setting.CameraSensitivity;
            float deltaAngleVertical = Mathf.Rad2Deg * Mathf.Atan(inputTransformUp.sqrMagnitude / _cameraTransform.forward.sqrMagnitude) * Time.deltaTime * setting.CameraSensitivity;
            Quaternion horizontalRotation = Quaternion.AngleAxis(deltaAngleHorizontal, input.x * _playerTransform.up);
            Quaternion verticalRotation = Quaternion.AngleAxis(deltaAngleVertical, input.y * -_cameraTransform.right);
            //min <= currentVerticalRotate + rotateVerticalAngle <= max なら垂直方向にカメラを回転
            float afterAngle = NegativeAngle(_cameraTransform.rotation.eulerAngles.x) + NegativeAngle(verticalRotation.eulerAngles.x);
            //Debug.Log(afterAngle);
            if (NegativeAngle(setting.RotationVerticalAngleDown) > afterAngle && afterAngle > NegativeAngle(setting.RotationVerticalAngleUp))
            {
                _cameraTransform.rotation = verticalRotation * _cameraTransform.rotation;
            }
            // 先にカメラを回転させないと,Playerの回転後にこの関数を抜けたあとにPlayerの子オブジェクトとしてのカメラが意図しない方向に回転してしまう
            _playerTransform.rotation = horizontalRotation * _playerTransform.rotation;
        }

        /// <summary>
        /// 0度から360度までの角度を-180から180度の角度の表示にする
        /// </summary>
        /// <returns></returns>
        private float NegativeAngle(float angle)
        {
            angle = angle - IntegerOfFloat(angle / 360f) * 360f;
            if (angle > 180f && angle < 360f + Mathf.Epsilon)
            {
                return angle - 360f;
            }
            return angle;
        }

        private float IntegerOfFloat(float f)
        {
            if (f < 0.0f) return -Mathf.Floor(-f);
            return Mathf.Floor(f);
        }

        /// <summary>
        /// 一人称視点のカメラの作成
        /// </summary>
        /// <param name="cameraTransform">回転するカメラ</param>
        /// <param name="playerTransform">カメラが追従するプレイヤー</param>
        // public FirstPersonCamera(Transform cameraTransform, Transform playerTransform, float sensitivity)
        // {
        //     _cameraTransform = cameraTransform;
        //     _playerTransform = playerTransform;
        //     _cameraSensitivity = sensitivity;
        //     //視点用のカメラをplayerの子オブジェクトに指定する.
        //     _cameraTransform.SetParent(_playerTransform);
        // }
        // public FirstPersonCamera(Transform cameraTransform, Transform playerTransform)
        // {
        //     _cameraTransform = cameraTransform;
        //     _playerTransform = playerTransform;
        //     _cameraSensitivity = 1.0f;
        //     //視点用のカメラをplayerの子オブジェクトに指定する.
        //     _cameraTransform.SetParent(_playerTransform);
        // }
    }

}