using UnityEngine;

namespace ItemSearchSystem
{
    public class SearchableTestItem : MonoBehaviour, ISearchable
    {
        public void OnSearch()
        {
            Debug.Log("Searched This Item");
        }
    }
}