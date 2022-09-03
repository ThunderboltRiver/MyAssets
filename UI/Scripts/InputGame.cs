using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputGame : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
  public Vector2 TouchRunPosition;
  public float dash_sensitivity;
  public FixedButton SitButton; //右画面JoyStick
  [HideInInspector]
  public Vector2 Direction;
  [HideInInspector]
  protected int PointerId;
  [HideInInspector]
  public bool Pressed;
  [HideInInspector]
  public GameObject handle;
  [HideInInspector]
  public Vector2 handle_origin;
  [HideInInspector]
  public bool Dragging;
  [HideInInspector]
  public Vector2 touch;
  private AudioSource FootStep;
  public Player player;






  // Start is called before the first frame update
  void Start()
  {

      TouchRunPosition = GameObject.Find("TouchRun").transform.position;
      handle = gameObject.transform.Find("Handle").gameObject;
      handle_origin = handle.transform.position;

  }

  // Update is called once per frame
  void Update()
  {
    if (Pressed){
        touch = Input.touches[PointerId].position;
        handle.transform.position = touch;
        Vector2 Direction_tmp = (touch - TouchRunPosition).normalized;
        if (SitButton.Pressed){
          Direction = Direction_tmp;
        }
        else{
          Direction = Direction_tmp * dash_sensitivity;

        }

      }
      else {
        Direction = new Vector2();

      }
      player.fps.RunAxis = Direction;


  }

  public void OnPointerDown(PointerEventData eventData)
  {
      Pressed = eventData.pointerCurrentRaycast.gameObject == gameObject;
      PointerId = eventData.pointerId;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
      Pressed = false;
      handle.transform.position = handle_origin;
  }

      //ドラッグした分だけオブジェクトを動かします。


}
