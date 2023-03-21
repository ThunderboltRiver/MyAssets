using UnityEngine;
namespace ItemSearchSystem
{
    public interface ITakable
    {
        public void OnTaken(Vector3 takeDirection);
    }
}