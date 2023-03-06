using UnityEngine;

namespace ItemSearchSystem
{
    public class SearchableTestItem : MonoBehaviour, ISearchable
    {
        public void OnSearch()
        {
            Debug.Log("This item was Searched");
        }
    }
}