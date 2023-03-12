using UnityEngine;
namespace ItemSearchSystem
{
    public class Searcher
    {
        public Transform Transform { get; }
        public float Radius { get; }
        public float MaxDistance { get; }

        public Vector3 SearchDirection => Transform.forward;

        public bool Search(out GameObject result)
        {
            if (Physics.SphereCast(Transform.position, Radius, SearchDirection, out RaycastHit hitInfo, MaxDistance))
            {
                GameObject gameObject = hitInfo.collider.gameObject;
                if (gameObject.TryGetComponent(out ISearchable searchable))
                {
                    searchable.OnSearch();
                    result = gameObject;
                    return true;
                }
            }
            result = null;
            return false;
        }
        public Searcher()
        {
            Transform = new GameObject().transform;
            Radius = 0.5f;
            MaxDistance = 1.0f;
        }
        public Searcher(Transform transform, float radius, float maxDistance)
        {
            Transform = transform;
            Radius = radius;
            MaxDistance = maxDistance;
        }
    }
}