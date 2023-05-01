using System;
using UnityEngine;

namespace ScreenChangeSystem
{
    public class ProgressMachine : MonoBehaviour
    {
        // Start is called before the first frame update
        public IObservable<string> CurrentGameProgress { get; private set; }

        public void AddStartProgress(int progressKey, GameProgress gameProgress)
        {
        }

        public void ChangeGameProgress(int progressKey)
        {
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
