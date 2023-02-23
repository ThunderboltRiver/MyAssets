using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace General.Views
{
    public class PressableUI : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<bool> isPressed => _isPressed;
        private readonly BoolReactiveProperty _isPressed = new BoolReactiveProperty(false);

        protected ObservableEventTrigger eventTrigger;

        void Awake()
        {
            eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();
        }
        void Start()
        {

            // PointerDown
            eventTrigger.OnPointerDownAsObservable()
            .Subscribe(pointerEventData => _isPressed.Value = true)
            .AddTo(this);

        }

        public void InitProperty()
        {
            _isPressed.Value = false;
        }
    }

}
