using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using General.View;

namespace MainGameScene.View
{
    [RequireComponent(typeof(Image))]
    public class InventryWindow : PressableUI
    {
        public readonly Image image;
        //public readonly int InventryNumber;

        void Start()
        {
            Image image = GetComponent<Image>();

        }
    }

}
