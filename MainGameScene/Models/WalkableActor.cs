using MainGameScene.Model;
using InputableActor;
using UnityEngine;
public class WalkableActor : Actor<Vector2>
{
    [SerializeField] private Walker _walker;
    protected override void OnStart()
    {
        _walker.AddComponents(GetComponent<Rigidbody>(), GetComponent<CapsuleCollider>());
        ChangeHandler(_walker);
    }

}