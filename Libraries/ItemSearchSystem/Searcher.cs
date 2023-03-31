using System;
using System.Linq;
using UnityEngine;
namespace ItemSearchSystem
{
    public abstract class Searcher
    {
        protected abstract GameObject[] Recognize();
        public GameObject[] CurrentRecognition { get; }
        public GameObject[] Search()
        {
            GameObject[] result = Recognize().Where(gameObject => gameObject.TryGetComponent<ISearchable>(out _)).ToArray();
            Array.ForEach(result, (GameObject gameObject) => { gameObject.GetComponent<ISearchable>()?.OnSearch(); });
            return result;
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