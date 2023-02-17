using UnityEngine;

namespace MainGameScene.Model
{
    public sealed class SlopeJudger
    {
        private CapsuleCollider _collider;
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

        public SlopeJudger(CapsuleCollider collider)
        {
            this._collider = collider;

        }
    }
}