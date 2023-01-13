using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainGameScene.View
{
    [RequireComponent(typeof(RectTransform))]
    public class MoveController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] RectTransform _handle;
        //private RectTransform _touchRun;
        //public FixedButton SitButton; //右画面JoyStick
        float radius;
        public Vector2 moveDirection { get; private set; } = Vector2.zero;
        private int PointerId;
        private bool Pressed;
        private Vector2 _handleOrigin;
        private Vector2 touch;





        // Start is called before the first frame update
        void Start()
        {
            radius = GetComponent<RectTransform>().sizeDelta.x / 2;
            _handleOrigin = _handle.position;
            Debug.Log(radius);

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Pressed)
            {
                touch = Input.touches[PointerId].position;
                Vector2 moveDirection_tmp = touch - _handleOrigin;
                if (moveDirection_tmp.sqrMagnitude <= (float)Math.Pow(radius, 2f))
                {
                    _handle.position = touch;
                    moveDirection = moveDirection_tmp / radius;
                }
                else
                {
                    _handle.position = radius * moveDirection_tmp.normalized + _handleOrigin;
                    moveDirection = moveDirection_tmp.normalized;
                }

                //Debug.Log(moveDirection);
            }
            else
            {
                moveDirection = Vector2.zero;

            }


        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = eventData.pointerCurrentRaycast.gameObject == gameObject;
            PointerId = eventData.pointerId;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
            _handle.position = _handleOrigin;
        }


    }

}
