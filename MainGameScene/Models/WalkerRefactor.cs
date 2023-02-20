using System;
using UnityEngine;
using InputableActor;
namespace MainGameScene.Model
{
    [Serializable]
    public class WalkerRefactor : InputHandlerRefactor<Vector2>
    {

        [Serializable]
        private class Setting
        {
            [SerializeField] private float _speed;
            [SerializeField] private float _maxAngle;

            public float Speed => _speed;
            public float MaxAngle => _maxAngle;
        }
        [SerializeField] private Rigidbody rigidbody;
        private SlopeJudger _slopeJudger;
        [SerializeField] private Setting setting;
        protected Vector2 inputedAcceleration;
        public bool isGrounded { get => _slopeJudger.GetNormalVectorOnPlane() != null; }

        public bool isMoving { get => isGrounded && rigidbody.velocity != Vector3.zero; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rigidbody">"移動対象のrigidbody"</param>
        /// <param name="slopeJudger">接地する平面を判定するオブジェクト</param>
        /// <param name="speed">移動速度の比率</param>
        /// <param name="maxAngle">登ることが可能な平面の最大角度</param>
        // public Walker(Rigidbody rigidbody, SlopeJudger slopeJudger, float speed, float maxAngle)
        // {
        //     this.rigidbody = rigidbody;
        //     this._slopeJudger = slopeJudger;
        //     this._speed = speed;
        //     this._maxAngle = maxAngle;
        // }
        public WalkerRefactor(Rigidbody rigidbody, SlopeJudger slopeJudger) : base()
        {
            this.rigidbody = rigidbody;
            this._slopeJudger = slopeJudger;
        }

        public WalkerRefactor(Rigidbody rigidbody, CapsuleCollider collider) : base()
        {
            this.rigidbody = rigidbody;
            this._slopeJudger = new(collider);
        }
        public WalkerRefactor()
        {

        }
        public override void LoadSetting<TSetting>(string settingKey, TSetting setting)
        {
            if (setting is nameof(rigidbody) && setting is Rigidbody _rigidbody) this.rigidbody = _rigidbody;
            if (setting is "collider" && setting is CapsuleCollider _collider) this._slopeJudger = new(_collider);
        }
        protected override void OnAwake()
        {
            inputQueueMask = 1;
        }
        protected override void OnUpdate()
        {
            //OnFixedUpdateの更新まで入力値は無視する
            Debug.Log($"inputQueue : {inputQueue.Count}");
        }

        protected override void OnFixedUpdate()
        {
            inputQueue.TryDequeue(out inputedAcceleration);
            if (IsValidVector(inputedAcceleration))
            {
                MoveRigidbody(rigidbody.transform.forward * inputedAcceleration.y + rigidbody.transform.right * inputedAcceleration.x);
            }
        }
        /// <summary>
        /// 等速直線運動をする
        /// </summary>
        /// <param name="rigidboy">力を加える対象</param>
        /// <param name="force">目的の加速度ベクトル</param>
        private void MoveRigidbody(Vector3 inputedAcceleration)
        {
            //平面に接地しているときに実行
            if (isGrounded && IsValidVector(inputedAcceleration))
            {
                // 物理計算の軽量化のための処理(なくても正常に動作する)
                // ベクトルが入力されていないときに実行
                if (inputedAcceleration == Vector3.zero)
                {
                    rigidbody.isKinematic = true;
                    return;
                }
                rigidbody.isKinematic = false;
                // 接地している平面の角度が_maxAngleをこえていなければ移動処理を行う
                if ((float)_slopeJudger.GetPlaneAngle() <= setting.MaxAngle)
                {
                    //rididbodyの移動
                    rigidbody.AddForce(AccelerationOnPlane(inputedAcceleration, (Vector3)_slopeJudger.GetNormalVectorOnPlane(), setting.Speed)
                        - rigidbody.velocity / Time.fixedDeltaTime, ForceMode.Acceleration);
                }
            }
        }

        /// <summary>
        /// 平面上で人間らしく歩行する際の加速度ベクトルを計算する
        /// </summary>
        /// <param name="originAccel">元々の加速度ベクトル</param>
        /// <param name="normalOnPlane">平面の単位法線ベクトル</param>
        /// <param name="rate">移動加速度の比率</param>
        /// <returns>加速度ベクトル</returns>
        private Vector3 AccelerationOnPlane(Vector3 originAccel, Vector3 normalOnPlane, float rate)
        {
            //平面上での加速度ベクトルを計算
            Vector3 Accelaration = rate * originAccel.sqrMagnitude * Vector3.ProjectOnPlane(originAccel, normalOnPlane);
            //平面に沿った方向の重力加速度を計算
            Vector3 gravityOnPlane = Vector3.ProjectOnPlane(Physics.gravity, normalOnPlane);
            //重力加速度を無視した加速度を返す
            return Accelaration - gravityOnPlane;
        }

        /// <summary>
        /// OnFixedUpdateにおける入力値バリデーション
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>bool</returns>
        private bool IsValidVector(Vector2 vector)
        {
            return vector.sqrMagnitude <= 1.02f;
        }
        /// <summary>
        /// MoveRigidbodyにおける入力値バリデーション
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>bool</returns>
        private bool IsValidVector(Vector3 vector)
        {
            return vector.sqrMagnitude <= 2.0f;
        }

        public void AddComponents(Rigidbody rigidbody, CapsuleCollider collider)
        {
            this.rigidbody = rigidbody;
            this._slopeJudger = new(collider);
        }

    }

}