using UnityEngine;
namespace ItemSearchSystem
{
    public interface ITakable
    {
        void OnTaken(Vector3 takeDirection);
        void OnSelected();
        void OnDeselected();
    }
}