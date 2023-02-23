using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General.Models
{
    public class SceneNavigater : MonoBehaviour
    {
        protected String _scene;
        public void LoadScene()
        {
            SceneManager.LoadScene(_scene);
        }
    }
}

