using System;
using UnityEngine;

namespace MainGameScene.Model
{
    public interface IRigidbodyMover
    {
        void MoveRigidbody(Rigidbody rigidboy, Vector3 force);
    }
}