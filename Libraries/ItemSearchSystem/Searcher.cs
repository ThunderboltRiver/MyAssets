using System;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;
namespace ItemSearchSystem
{
    public abstract class Searcher
    {
        private readonly ReactiveCollection<GameObject> _currentRecognition = new();
        protected abstract GameObject[] Recognize();
        public IReadOnlyReactiveCollection<GameObject> CurrentRecognition => _currentRecognition;
        public void Search()
        {
            GameObject[] recognition = Recognize().Where(gameObject => gameObject.TryGetComponent<ISearchable>(out _)).ToArray();
            Desearch(gameObject => !recognition.Contains(gameObject));
            Array.ForEach(recognition, (GameObject gameObject) =>
            {
                _currentRecognition.Add(gameObject);
                gameObject.GetComponent<ISearchable>().OnSearch();
            });
        }
        public void Desearch(Func<GameObject, bool> condition)
        {
            if (_currentRecognition != null)
            {
                Array.ForEach(_currentRecognition.Where(gameObject => gameObject.TryGetComponent<ISearchable>(out _) && condition(gameObject)).ToArray()
                , gameObject =>
                {
                    _currentRecognition.Remove(gameObject);
                    gameObject.GetComponent<ISearchable>().OnDesearch();
                });
            }
        }
    }

    public class SphereSearcher : Searcher
    {
        public Transform Transform { get; }
        public float Radius { get; }

        protected override GameObject[] Recognize()
        {
            return Physics.OverlapSphere(Transform.position, Radius).Select(collider => collider.gameObject).ToArray();
        }
        public SphereSearcher(Transform transform, float radius)
        {
            Transform = transform;
            Radius = radius;
        }

    }
}