using UnityEngine;
namespace ItemSearchSystem
{
    public class Searcher
    {
        private Vector3 origin = Vector3.zero;
        private float radius = 0.5f;
        private float maxDistance = 1.0f;
        private Vector3 direction = Vector3.forward;

        public bool Search(out GameObject result)
        {
            if (Physics.SphereCast(origin, radius, direction, out RaycastHit hitInfo, maxDistance))
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
    }
}