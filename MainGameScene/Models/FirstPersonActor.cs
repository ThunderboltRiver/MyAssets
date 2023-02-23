using System;
using System.Reflection.Emit;
using UnityEngine;
using InputableActor;

namespace MainGameScene.Model
{

    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class FirstPersonActor : Actor<FirstPersonActor.AcceptableKeys, Vector2>
    {
        [SerializeField] private Walker _walker;
        [SerializeField] private FirstPersonCamera _fpsCamera;
        //[SerializeField] private Transform _cameraTransform;s

        public enum AcceptableKeys
        {
            MoveRigidbody,
            RotateCamera,
        }
        protected override void OnStart()
        {
            Debug.Log(nameof(AcceptableKeys.MoveRigidbody));
            _walker.AddComponents(GetComponent<Rigidbody>(), GetComponent<CapsuleCollider>());
            AddInputHandler(AcceptableKeys.MoveRigidbody, _walker);
            AddInputHandler(AcceptableKeys.RotateCamera, _fpsCamera);
        }

        public void SettingCameraSensitivity(float value)
        {
            _fpsCamera.CameraSensitivity = value;
        }
    }
}