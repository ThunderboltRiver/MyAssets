using System.Runtime.CompilerServices;
using UnityEngine;
namespace ItemSearchSystem
{
    public class TakableAddPresenter : MonoBehaviour
    {
        Taker taker;
        GameObject view;
        public void ChangeTaker(Taker taker)
        {
            this.taker = taker;
        }
        public void ChangeView(GameObject view)
        {
            this.view = view;
        }

        void Start()
        {
            taker.WaitingTakablesAsObservableCollection.CreateView(m =>
            {
                view.SetActive(true);
                return view.transform;
            });
        }
    }
}