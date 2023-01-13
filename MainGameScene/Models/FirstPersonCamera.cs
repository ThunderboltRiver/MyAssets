#define DEBUG
using System;
using UnityEngine;

namespace MainGameScene.Model
{
    public class FirstPersonCamera
    {
        private Transform _playerTransform;
        private Transform _cameraTransform;
        public void HandleInput(Vector2 input)
        {
            if (IsValidInput(input))
            {
                RotateCamera(input);
            }
        }

        private bool IsValidInput(Vector2 input)
        {
            return input.sqrMagnitude <= 1.0f + Mathf.Epsilon;
        }
        private void RotateCamera(Vector2 input)
        {
            Vector3 inputTransform = input.x * _cameraTransform.right - input.y * _cameraTransform.up;
            Vector3 rotateAxis = Vector3.Cross(inputTransform, _cameraTransform.forward).normalized;
            float rotateAngle = Mathf.Rad2Deg * Mathf.Atan(inputTransform.sqrMagnitude / _cameraTransform.forward.sqrMagnitude);
            _cameraTransform.RotateAround(_playerTransform.position, rotateAxis, rotateAngle);
        }
        public FirstPersonCamera(Transform cameraTransform, Transform playerTransform)
        {
            _cameraTransform = cameraTransform;
            _playerTransform = playerTransform;
        }
    }

}