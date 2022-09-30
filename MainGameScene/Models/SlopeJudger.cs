using UnityEngine;

namespace MainGameScene.Model
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class SlopeJudger : MonoBehaviour
    {
        private CapsuleCollider _collider;
        [SerializeField] float _maxAngle;
        private float _castDistance;
        private Vector3 _normalVector;
        public Vector3 normalVector
        {
            get { return _normalVector; }

        }

        void Start()
        {
            _collider = GetComponent<CapsuleCollider>();
            _castDistance = _collider.height / 2f - _collider.radius + 0.01f;
        }
        private Vector3 NormalVector()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, _collider.radius, Vector3.down,
            out hitInfo, _castDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                return hitInfo.normal;
            }
            return Vector3.up;

        }
        public bool IsWalkableSlope()
        {
            _normalVector = NormalVector();
            Debug.Log(_normalVector);
            return Vector3.Angle(_normalVector, Vector3.up) < _maxAngle;

        }
    }
}