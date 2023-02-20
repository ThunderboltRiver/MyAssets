using UnityEngine;
using InputableActor;

namespace MainGameScene.Model
{

    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class FirstPersonActorRefactor : ActorRefactor<int, Vector2>
    {
        [SerializeField] private WalkerRefactor _walker;
        [SerializeField] private FirstPersonCameraRefactor _fpsCamera;
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