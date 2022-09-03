using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventryWindow : MonoBehaviour, IPointerDownHandler
{
    private GameObject UIObject;
    private GameObject fps;
    public Take take;
    public Use use;
    public Drop drop;
    private static string[] Inventry = new string[]{"Inventry0", "Inventry1", "Inventry2", "Inventry3"};
    private static Image[] InventImages = new Image[Inventry.Length];
    private List<GameObject> items = new List<GameObject>(){};
    private int InventNum;
    private bool isitemsUpdated;
    [HideInInspector]
    public bool isContaining = false;

    // Start is called before the first frame update
    void Start()
    {
      fps = GameObject.Find("RigidBodyFPSController");
      InventImages = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
      isitemsUpdated = false;
      if (take.Pressed && !take.itemDeactivate)GetItem(take.item);
      if(use.Pressed && isContaining)UseItem(InventNum);
      if(drop.Pressed && isContaining)DropItem(InventNum);
      if(isitemsUpdated)ImageChanger();
    }

    public void OnPointerDown(PointerEventData eventData){
      UIObject = eventData.pointerPressRaycast.gameObject;
      bool isInventry = UIObject.CompareTag("Inventry");
      if (isInventry){
        InventNum = Array.IndexOf(Inventry, UIObject.name);
        isContaining = InventNum < items.Count;
      }
    }

    public void UseItem(int InventNum){
      switch(items[InventNum].name){
        case "tama":
          Debug.Log("used tama");
          break;
      }
      items.RemoveAt(InventNum);
      isContaining = false;
      isitemsUpdated = true;
    }

    public void DropItem(int InventNum){
      switch(items[InventNum].name){
        case "tama":
          Debug.Log("drop tama");
          break;
      }
      GameObject SelectedItem = items[InventNum];
      SelectedItem.SetActive(true);
      SelectedItem.transform.parent = null;
      items.RemoveAt(InventNum);
      isContaining = false;
      isitemsUpdated = true;
    }

    public void GetItem(GameObject item){
      if(items.Count == transform.childCount) Debug.Log("これ以上は持てない");
      else {
        items.Add(item);
        item.transform.parent = fps.transform;
        item.transform.localPosition = Vector3.zero;
        item.SetActive(false);
        take.itemDeactivate = !item.activeSelf;
        Debug.Log("Get item");
        isitemsUpdated = true;
      }
    }

    public void ImageChanger(){
      var index = 1;
      Debug.Log("change");
      foreach(Image image in InventImages){
        image.sprite = null;
        image.color = new Color(255, 255, 255, 0);
      }
      if(items.Count > 0){
        foreach(GameObject item in items){
          Sprite sprite = Resources.Load<Sprite>($"{item.name}-icon");
          Image image = InventImages[index++];
          Color c = image.color;
          image.sprite = sprite;
          image.color = new Color(c.r, c.g, c.b, 1);
          image.type = Image.Type.Simple;
          image.preserveAspect = true;
        }
      }
    }
}
