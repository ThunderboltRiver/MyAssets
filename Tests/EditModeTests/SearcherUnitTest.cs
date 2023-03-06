using ItemSearchSystem;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace EditModeTests
{
    public class SearcherUnitTest
    {
        readonly Searcher searcher = new();
        readonly GameObject targetObject = new();
        /// <summary>
        /// テストシーンの作成
        /// </summary>
        [SetUp]
        public void Setup()
        {
            SphereCollider collider = targetObject.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = 1.0f;
            targetObject.transform.position = 2.0f * Vector3.forward;
        }

        [Test]
        public void SearcherがISearchableなGameObjectを発見できるか()
        {
            _ = targetObject.AddComponent<SearchableTestItem>();
            Assert.True(searcher.Search(out GameObject gameObject));
            Assert.True(gameObject.TryGetComponent(out ISearchable _));
        }
        [Test]
        public void ISearchableを発見するとOnSearchを実行するか()
        {
            _ = targetObject.AddComponent<SearchableTestItem>();
            LogAssert.Expect(LogType.Log, "This item was Searched");
            searcher.Search(out GameObject _);
        }

        [Test]
        public void ISearchableなGameObjectを返さないときもあるか()
        {
            Assert.False(searcher.Search(out GameObject _));
        }
    }
}