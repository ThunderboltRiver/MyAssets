using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class Take : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    // Start is called before the first frame update
    [HideInInspector]
    public bool Pressed = false;
    [HideInInspector]
    public bool act = false;
    [HideInInspector]
    public GameObject item;
    public Player player;
    private Image image;
    [HideInInspector]
    public bool itemDeactivate;

    void Start()
    {
      image = gameObject.GetComponent<Image>();
      image.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
      if (player.isSelecting && !act){
        act = true;
        image.enabled = act;
      }else if(!player.isSelecting && act){
        act = false;
        image.enabled = act;
      }

    }

    public void OnPointerDown(PointerEventData eventData){
      //act = false;
      //text.enabled = act;
      Debug.Log("take Pressed");
      player.isSelecting = false;
      //item.transform.parent = fps.transform;
      //item.SetActive(false);
      //itemDeactivate = true;
      Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData){
      Debug.Log("take Up");
      Pressed = false;
      itemDeactivate = false;
    }
}
