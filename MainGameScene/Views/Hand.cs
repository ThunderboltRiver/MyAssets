using System;
using System.Collections.Generic;
using General.View;
using UnityEngine;
using UniRx;

namespace MainGameScene.View
{
    public class Hand : PressableUI
    {
        public readonly BoolReactiveProperty isActive = new BoolReactiveProperty(false);

        void Activater()
        {
            isActive.Value = true;
        }
    }
}