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
        private Vector3 _inputedDirection;
        private bool isGrounded;
        public Vector3 Velocity
        {
            get { return _rigidbody.velocity; }
        }

        public bool Grounded
        {
            get { return isGrounded; }
        }

        public bool Jumping
        {
            get { return !isGrounded; }
        }

        public bool Running
        {
            get
            {
#if !MOBILE_INPUT
                return isGrounded && _rigidbody.velocity != Vector3.zero;
#else
	            return false;
#endif
            }
        }

        [SerializeField] private float Speed;
        [SerializeField] private float _maxAngle;
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _slopeJudger = new SlopeJudger(_capsuleCollider);
        }

        private void FixedUpdate()
        {
            isGrounded = _slopeJudger.GetNormalVectorOnPlane() != null;
            //平面に接地しているときに実行
            if (isGrounded)
            {
                // 物理計算の軽量化のための処理(なくても正常に動作する)
                // ベクトルが入力されていないときに実行
                if (_inputedDirection == Vector3.zero)
                {
                    _rigidbody.isKinematic = true;
                    return;
                }
                _rigidbody.isKinematic = false;

                // 接地している平面の角度が_maxAngleをこえていなければ移動処理を行う
                if ((float)_slopeJudger.GetPlaneAngle() <= _maxAngle)
                {
                    //平面上の単位法線ベクトルを取得
                    Vector3 normalOnPlane = (Vector3)_slopeJudger.GetNormalVectorOnPlane();
                    //rididbodyの移動
                    MoveRigidbody(_rigidbody, AccelerationOnPlane(_inputedDirection, normalOnPlane, Speed));
                }
            }
        }

        //入力を受け付ける窓口
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
        /// <param name="force">目的の加速度ベクトル</param>
        public void MoveRigidbody(Rigidbody rigidbody, Vector3 force)
        {
            rigidbody.AddForce(force - rigidbody.velocity / Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        private bool IsValidVector(Vector2 input)
        {
            return input.sqrMagnitude <= 1.02f;
        }

        /// <summary>
        /// 平面上で人間らしく歩行する際の加速度ベクトルを計算する
        /// </summary>
        /// <param name="originAccel">元々の加速度ベクトル</param>
        /// <param name="normalOnPlane">平面の単位法線ベクトル</param>
        /// <param name="rate">移動加速度の比率</param>
        /// <returns></returns>
        private Vector3 AccelerationOnPlane(Vector3 originAccel, Vector3 normalOnPlane, float rate)
        {
            //平面上での加速度ベクトルを計算
            Vector3 Accelaration = rate * originAccel.sqrMagnitude * Vector3.ProjectOnPlane(originAccel, normalOnPlane);
            //平面に沿った方向の重力加速度を計算
            Vector3 gravityOnPlane = Vector3.ProjectOnPlane(Physics.gravity, normalOnPlane);
            //重力加速度を無視した加速度を返す
            return Accelaration - gravityOnPlane;
        }

    }
}

