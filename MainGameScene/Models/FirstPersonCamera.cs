#define DEBUG
using System;
using UnityEngine;
using SerializableClass;
using InputableActor;

namespace MainGameScene.Model
{
    [Serializable]
    public class FirstPersonCamera : InputHandler<Vector2>
    {
        const float MinCameraSensitivity = 0.0f;
        const float MaxCameraSensitivity = 1.0f;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _playerTransform;
        [Range(MinCameraSensitivity, MaxCameraSensitivity)][SerializeField] float _cameraSensitivity;
        [SerializeField] float _RotationVerticalAngleUp = -90.0f;
        [SerializeField] float _RotationVerticalAngleDown = 90.0f;
        public float CameraSensitivity
        {
            get => _cameraSensitivity;
            set
            {
                if (MinCameraSensitivity <= value && value <= MaxCameraSensitivity)
                {
                    _cameraSensitivity = value;
                }
            }
        }
        public float RotationVerticalAngleUp => _RotationVerticalAngleUp + Mathf.Epsilon;
        public float RotationVerticalAngleDown => _RotationVerticalAngleDown - Mathf.Epsilon;

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
            float deltaAngleHorizontal = Mathf.Rad2Deg * Mathf.Atan(inputTransformRight.sqrMagnitude / _cameraTransform.forward.sqrMagnitude) * Time.deltaTime * CameraSensitivity;
            float deltaAngleVertical = Mathf.Rad2Deg * Mathf.Atan(inputTransformUp.sqrMagnitude / _cameraTransform.forward.sqrMagnitude) * Time.deltaTime * CameraSensitivity;
            Quaternion horizontalRotation = Quaternion.AngleAxis(deltaAngleHorizontal, input.x * _playerTransform.up);
            Quaternion verticalRotation = Quaternion.AngleAxis(deltaAngleVertical, input.y * -_cameraTransform.right);
            //min <= currentVerticalRotate + rotateVerticalAngle <= max なら垂直方向にカメラを回転
            float afterAngle = NegativeAngle(_cameraTransform.rotation.eulerAngles.x) + NegativeAngle(verticalRotation.eulerAngles.x);
            //Debug.Log(afterAngle);
            if (NegativeAngle(RotationVerticalAngleDown) > afterAngle && afterAngle > NegativeAngle(RotationVerticalAngleUp))
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

    }

}