using UnityEngine;

namespace MainGameScene.Model
{
    [RequireComponent(typeof(Rigidbody))]
    public class Walker : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Vector3 _movingDirection = Vector3.zero;
        [SerializeField] private float Speed;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(_movingDirection * Speed, ForceMode.Impulse);
            Debug.Log("add");
        }

        public void MovingDirection(Vector2 input, Vector3 normalVector)
        {
            _movingDirection = transform.forward * input.y + transform.right * input.x;
            _movingDirection = Vector3.ProjectOnPlane(_movingDirection, normalVector).normalized;
            Debug.Log(_movingDirection);
        }



    }
}

