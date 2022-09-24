using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainGameScene.View
{
    public class ControlHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] RectTransform _handle;
        [SerializeField] RectTransform _touchRun;
        //public FixedButton SitButton; //右画面JoyStick
        public Vector2 Direction { get; private set; } = Vector2.zero;
        private int PointerId;
        private bool Pressed;
        private Vector2 _handleOrigin;
        private Vector2 touch;





        // Start is called before the first frame update
        void Start()
        {
            _handleOrigin = _handle.position;
            Debug.Log(_handleOrigin);
        }

        // Update is called once per frame
        void Update()
        {
            if (Pressed)
            {
                touch = Input.touches[PointerId].position;
                Vector2 Direction_tmp = touch - _handleOrigin;
                float touchRunRadius = _touchRun.sizeDelta.x;
                if (Direction_tmp.sqrMagnitude <= (float)Math.Pow(touchRunRadius, 2f))
                {
                    _handle.position = touch;

                }
                else
                {
                    _handle.position = touchRunRadius * Direction_tmp.normalized + _handleOrigin;
                }
                Direction = Direction_tmp.normalized;
            }
            else
            {
                Direction = Vector2.zero;

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
