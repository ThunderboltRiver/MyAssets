using UnityEngine;
using MainGameScene.View;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

using Vector2 = UnityEngine.Vector2;
public class TachableUI
{
    private int targetID;

    public IEnumerable<Touch> getTargetFingers()
    {
        EventSystem currentEvent = EventSystem.current;
        PointerEventData eventData = new PointerEventData(currentEvent);
        List<Touch> targetFingers = new() { };
        foreach (Touch touch in Input.touches)
        {
            eventData.position = touch.position;
            List<RaycastResult> _rayResults = new List<RaycastResult>() { };
            currentEvent.RaycastAll(eventData, _rayResults);
            int targetResultsCount = _rayResults.Count(result => result.gameObject.GetInstanceID() == targetID && _rayResults.IndexOf(result) == 0);
            if (targetResultsCount > 0)
            {
                Debug.Log(targetResultsCount);
                targetFingers.Add(touch);
            }
        }
        return (IEnumerable<Touch>)targetFingers;
    }

    public TachableUI(int targetID)
    {
        this.targetID = targetID;
    }

}

[RequireComponent(typeof(RectTransform))]
public class StrokableArea : PublishableActor<Vector2>
{
    private RectTransform target;
    private TachableUI tachableUI;

    public void Start()
    {
        target = gameObject.GetComponent<RectTransform>();
        tachableUI = new(target.gameObject.GetInstanceID());
    }

    public override Vector2 Publish()
    {
        IEnumerable<Touch> targetfingers = tachableUI.getTargetFingers();
        if (targetfingers.Count() == 1) return targetfingers.First().deltaPosition;
        //もし指定範囲内に指が押されて,その範囲内で指が動いたら return deltaPosition
        return Vector2.zero;
    }
}