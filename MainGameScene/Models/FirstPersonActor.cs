using UnityEngine;
using InputableActor;

namespace MainGameScene.Model
{

    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class FirstPersonActor : Actor<int, Vector2>
    {
        [SerializeField] private Walker _walker;
        [SerializeField] private FirstPersonCamera _fpsCamera;
        //[SerializeField] private Transform _cameraTransform;s

        public enum AcceptableKeys
        {
            MoveRigidbody,
            RotateCamera,
        }
        void Awake()
        {

            //_fpsCamera = new(_cameraTransform, GetComponent<Rigidbody>().transform);

        }
        protected override void OnStart()
        {
            _walker.AddComponents(GetComponent<Rigidbody>(), GetComponent<CapsuleCollider>());
            AddInputHandler((int)AcceptableKeys.MoveRigidbody, _walker);
            AddInputHandler((int)AcceptableKeys.RotateCamera, _fpsCamera);
        }
    }
}