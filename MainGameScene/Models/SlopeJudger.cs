using UnityEngine;

namespace MainGameScene.Model
{
    //[RequireComponent(typeof(CapsuleCollider))]
    sealed class SlopeJudger//: MonoBehaviour
    {
        private CapsuleCollider _collider;
        // float _maxAngle;
        // private float _castDistance;
        // private Vector3 _normalVector;
        // public Vector3 normalVector
        // {
        //     get { return _normalVector; }

        // }

        // void Start()
        // {
        //     _collider = GetComponent<CapsuleCollider>();
        //     //Debug.Log(transform.position);
        // }
        public Vector3? GetNormalVectorOnPlane()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(_collider.transform.position, _collider.radius, Vector3.down,
            out hitInfo, _collider.height / 2f - _collider.radius + 0.01f))
            {
                return hitInfo.normal;
            }
            return null;

        }
        public float? GetPlaneAngle()
        {
            if (GetNormalVectorOnPlane() == null) return null;
            return Vector3.Angle((Vector3)GetNormalVectorOnPlane(), Vector3.up);
        }

        // public bool IsWalkableSlope()
        // {
        //     GetNormalVectorOnPlane();
        //     return Vector3.Angle(_normalVector, Vector3.up) < _maxAngle;

        // }

        public SlopeJudger(CapsuleCollider collider)
        {
            this._collider = collider;
            //this._maxAngle = maxAngle;
        }
    }
}