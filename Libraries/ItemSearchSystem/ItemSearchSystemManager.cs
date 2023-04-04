using UnityEngine;
using UniRx;

namespace ItemSearchSystem
{
    public class ItemSearchSystemManager : MonoBehaviour
    {
        private Searcher searcher;
        private Taker taker;
        private Register register;

        public IReadOnlyReactiveCollection<GameObject> CurrentRecognition => searcher.CurrentRecognition;
        public IReadOnlyReactiveCollection<GameObject> CurrentSelection => taker.CurrentSelections;
        public void Awake()
        {
            searcher = new SphereSearcher(this.gameObject.transform, 2.0f);
            taker = new RayCastingTaker(this.gameObject.transform, 2.0f);
            register = new(gameObject);
        }
        public void FixedUpdate()
        {
            SearchAndSelect();
        }

        public void SearchAndSelect()
        {
            searcher.Search();
            taker.Select(searcher.CurrentRecognition);
        }

        public void TakeAndRegsit(int selectionIndex)
        {
            if (taker.Take(selectionIndex, out GameObject takable) && register.TryRegist(takable))
            {
                AfterRegist(takable);
            };
        }

        protected virtual void AfterRegist(GameObject gameObject)
        {
            gameObject.transform.SetParent(this.gameObject.transform);
            gameObject.SetActive(false);
        }

    }

}