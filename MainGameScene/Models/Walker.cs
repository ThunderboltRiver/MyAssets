using System.Threading;
using UnityEngine;

namespace MainGameScene.Model
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Walker : MonoBehaviour, IRigidbodyMover, ISubscriber<Vector2>
    {
        private Rigidbody _rigidbody;
        private CapsuleCollider _capsuleCollider;
        private SlopeJudger _slopeJudger;
        private Vector3 _movingDirection;
        private Vector3 _inputedDirection;
        [SerializeField] private float Speed;
        [SerializeField] private float _maxAngle;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _slopeJudger = new SlopeJudger(_capsuleCollider);
            //Debug.Log(_rigidbody.position);
        }

        private void FixedUpdate()
        {
            bool isGrounded = _slopeJudger.GetNormalVectorOnPlane() != null;
            if (isGrounded)
            {
                if (_inputedDirection == Vector3.zero)
                {
                    _rigidbody.isKinematic = true;
                    return;
                }
                _rigidbody.isKinematic = false;
                float? _planeAngle = _slopeJudger.GetPlaneAngle();
                if (_planeAngle != null && _planeAngle <= _maxAngle)
                {
                    Vector3 normalOnPlane = (Vector3)_slopeJudger.GetNormalVectorOnPlane();
                    //坂を登るための処理 
                    _movingDirection = Vector3.ProjectOnPlane(_inputedDirection, normalOnPlane).normalized;
                    Vector3 gravityOnPlane = Mathf.Cos((float)_planeAngle * Mathf.Rad2Deg) * Physics.gravity.magnitude * normalOnPlane.normalized + Physics.gravity;
                    //Debug.Log(gravityOnPlane);
                    //Debug.Log(_movingDirection);
                    //_rigidbody.AddForce(-gravityOnPlane, ForceMode.Acceleration);
                    MoveRigidbody(_rigidbody, Speed * _inputedDirection.sqrMagnitude * _movingDirection - gravityOnPlane);
                }
            }
        }

        public void Subscribe(Vector2 input)
        {
            if (IsValidVector(input))
            {
                _inputedDirection = transform.forward * input.y + transform.right * input.x;
                return;
            }
            _inputedDirection = Vector3.zero;
        }
        /// <summary>
        /// IRigidbodyMover.MoveRigidbodyの実装
        /// 等速直線運動をするように実装
        /// 速度微調整のためのメンバ変数Speedを使用
        /// </summary>
        /// <param name="rigidboy">力を加える対象</param>
        /// <param name="force">目的の速度ベクトル</param>
        public void MoveRigidbody(Rigidbody rigidboy, Vector3 force)
        {
            _rigidbody.AddForce(force - _rigidbody.velocity / Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        private bool IsValidVector(Vector2 input)
        {
            //validation
            return true;
        }

    }
}

