using MainGameScene.Model;
using InputableActor;
using UnityEngine;
public class WalkableActor : ActorRefactor<Vector2>
{
    [SerializeField] private WalkerRefactor _walker;
    protected override void OnStart()
    {
        _walker.AddComponents(GetComponent<Rigidbody>(), GetComponent<CapsuleCollider>());
        ChangeHandler(_walker);
    }

}