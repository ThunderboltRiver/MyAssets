using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace StartScene.View
{
    public class StartButton : MonoBehaviour, IPointerDownHandler
    {
        public readonly BoolReactiveProperty isPressed = new BoolReactiveProperty(false);
        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed.Value = true;
        }
    }

}
