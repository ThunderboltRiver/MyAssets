using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// public class FixedButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
// {
//     [HideInInspector]
//     public bool Pressed;
//     public Player player;

//     // Use this for initialization
//     //void Start()
//     //{

//     //}

//     // Update is called once per frame
//     void Update()
//     {
//       if (Pressed){
//         player.PlayerView.transform.localPosition = Vector3.zero;        
//       }
//       else{
//         player.PlayerView.transform.localPosition = player.PlayerView_origin;
//       }
//     }

//     public void OnPointerDown(PointerEventData eventData)
//     {
//         Pressed = true;
//     }

//     public void OnPointerUp(PointerEventData eventData)
//     {
//         Pressed = false;
//     }
// }
