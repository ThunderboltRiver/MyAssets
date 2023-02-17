using UnityEngine;

namespace MainGameScene.Model
{
    public interface IRigidbodyMover
    {
        public Rigidbody rigidbody { get; }
        public bool isMoving { get; }
        public void MoveRigidbody(Vector3 force);
    }
}