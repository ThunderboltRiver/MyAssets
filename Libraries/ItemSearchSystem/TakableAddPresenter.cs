using System.Collections.Specialized;
using ObservableCollections;
using UniRx;
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
            taker.CurrentSelections.ObserveAdd().Subscribe(gameObject =>
            {
                view.SetActive(true);
            })
            .AddTo(this);

        }
    }
}