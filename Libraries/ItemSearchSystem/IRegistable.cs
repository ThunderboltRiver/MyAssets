using UnityEngine;

namespace ItemSearchSystem
{
    public interface IRegistable
    {
        int MaxRegistalbe { get; }
        void OnRegist(GameObject Owner);
    }
}