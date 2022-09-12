using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using General.Model;

namespace StartScene.Model
{
    public class MainGameNavigater : SceneNavigater
    {
        void Start()
        {
            this._scene = "MainGame";
        }

    }
}

