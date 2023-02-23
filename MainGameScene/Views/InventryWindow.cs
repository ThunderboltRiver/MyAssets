using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using General.Views;

namespace MainGameScene.View
{
    [RequireComponent(typeof(Image))]
    public class InventryWindow : PressableUI
    {
        //public readonly int InventryNumber;
        public Image image;
        public ReactiveProperty<Sprite> sprite = new ReactiveProperty<Sprite>();
        void Awake()
        {
            image = GetComponent<Image>();
        }

        void Start()
        {
            sprite
            .Subscribe(window =>
            {
                image.sprite = window;
                image.color = new Color(1.0f, 1.0f, 1.0f, window == null ? 0.0f : 1.0f);
            }).AddTo(this);

        }
    }

}
