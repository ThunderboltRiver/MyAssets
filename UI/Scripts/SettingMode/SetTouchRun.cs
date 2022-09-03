using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SetTouchRun : MonoBehaviour, IDragHandler
{
    [HideInInspector]
    public Vector2 UIPos;
    private RectTransform rectTransform;
    private GameObject TouchRun;

    // Start is called before the first frame update

    void Start()
    {
      rectTransform = transform.GetComponent<RectTransform>();
      rectTransform.anchoredPosition = Vector2.zero;
      TouchRun = GameObject.Find("TouchRun");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnDrag(PointerEventData eventData){
      int PointerId = eventData.pointerId;
      Vector2 touch = Input.touches[PointerId].position;
      transform.position = touch;

    }
    private Vector2 GetLocalPosition(Vector2 screenPosition){
      return transform.InverseTransformPoint(screenPosition);
    }
}
