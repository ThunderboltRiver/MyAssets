using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace General.View
{
    public class PressableUI : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<bool> isPressed => _isPressed;
        private readonly BoolReactiveProperty _isPressed = new BoolReactiveProperty(false);

        void Start()
        {
            ObservableEventTrigger eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();

            // PointerDown
            eventTrigger.OnPointerDownAsObservable()
            .Subscribe(pointerEventData => _isPressed.Value = true);

        }

        public void InitProperty()
        {
            _isPressed.Value = false;
        }
    }

}
