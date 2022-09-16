using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace General.View
{
    public class PressableUI : MonoBehaviour
    {
        public readonly BoolReactiveProperty isPressed = new BoolReactiveProperty(false);

        void Start()
        {
            ObservableEventTrigger eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();

            // PointerDown
            eventTrigger.OnPointerDownAsObservable()
            .Subscribe(pointerEventData => isPressed.Value = true);

        }
    }

}
