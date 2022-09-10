using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Take : MonoBehaviour, IPointerDownHandler{
  [HideInInspector]
  public GameObject takenitem;
  public Player player;
  private Image image;
  [HideInInspector]
  public bool itemDeactivate;

  void Start()
  {
    image = gameObject.GetComponent<Image>();
    image.enabled = false;

  }
  void Update()
  {
    image.enabled = !(player.selectingitem == null);
  }

  public void OnPointerDown(PointerEventData eventData){
    takenitem = player.selectingitem;
    takenitem.transform.parent = player.transform;
    takenitem.transform.localPosition = Vector3.zero;
    takenitem.SetActive(false);
    player.selectingitem = null;
  }
}
