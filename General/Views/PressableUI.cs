using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace General.View
{
    public class PressableUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public readonly BoolReactiveProperty isPressed = new BoolReactiveProperty(false);
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed.Value = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed.Value = false;
        }
    }

}
