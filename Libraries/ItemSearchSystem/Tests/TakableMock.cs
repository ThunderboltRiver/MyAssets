using UnityEngine;

namespace ItemSearchSystem
{

    public class TakableMock : MonoBehaviour, ITakable
    {
        public void OnTaken()
        {
            Debug.Log("TakableMock was Taken");
        }
    }

}