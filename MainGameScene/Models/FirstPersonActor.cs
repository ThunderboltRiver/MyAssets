using UnityEngine;

namespace MainGameScene.Model
{

    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class FirstPersonActor : InputableActor<Vector2, int>
    {
        private IInputHandler<Vector2> _walker;
        private IInputHandler<Vector2> _fpsCamera;
        [SerializeField] private float _speed;
        [SerializeField] private float _maxAngle;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _cameraSensitivity;

        public enum AcceptableKeys
        {
            MoveRigidbody,
            RotateCamera,
        }
        void Start()
        {
            // _walker = new Walker(GetComponent<Rigidbody>(), new SlopeJudger(GetComponent<CapsuleCollider>()), _speed, _maxAngle);
            // _fpsCamera = new FirstPersonCamera(_cameraTransform, GetComponent<Rigidbody>().transform, _cameraSensitivity);
            // AddInputHandler(_walker, (int)AcceptableKeys.MoveRigidbody);
            // AddInputHandler(_fpsCamera, (int)AcceptableKeys.RotateCamera);
        }
    }
}