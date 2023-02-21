using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using General.Singletons;

namespace General.Models
{
    public class UserInterfaceChanger : MonoSingleton<UserInterfaceChanger>
    {
        [SerializeField] private string currentCanvasKey;
        [SerializeField] private Canvas currentCanvas;

        [SerializeField] private SerializableDictionary<string, Canvas> nextCanvasDict;

        public void ChangeCanvas(string canvasKey)
        {
            currentCanvas.enabled = false;
            Canvas nextCanvas;
            if (nextCanvasDict.TryGetValue(canvasKey, out nextCanvas))
            {
                nextCanvas.enabled = true;
                currentCanvas = nextCanvas;
            }
        }

        public void RegisterCanvas(string canvasKey, Canvas canvas)
        {
            canvas.enabled = false;
            nextCanvasDict.TryAdd(canvasKey, canvas);
        }
        protected override void OnAwake()
        {
            foreach (Canvas canvas in nextCanvasDict.Values)
            {
                canvas.enabled = false;
            }
            if (!nextCanvasDict.TryAdd(currentCanvasKey, currentCanvas))
            {
                Debug.Log("Prease configure the initial currentCanvas");
            }
        }

    }


}