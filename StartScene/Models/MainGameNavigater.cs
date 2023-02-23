using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using General.Models;

namespace StartScene.Models
{
    public class MainGameNavigater : SceneNavigater
    {
        void Start()
        {
            this._scene = "MainGame";
        }

    }
}

