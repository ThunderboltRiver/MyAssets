using UnityEngine;

namespace General.Views
{
    public class CanvasKeySender : PointerUpUI
    {
        [SerializeField] private string _canvasName;

        public string CanvasName => _canvasName;
        protected override void OnStart()
        {
            Debug.Log(_canvasName);
        }

    }

}