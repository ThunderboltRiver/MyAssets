using ItemSearchSystem;
using UnityEngine;
using UnityEditor.SceneManagement;
using NUnit.Framework;
using NSubstitute;

namespace EditModeTests
{
    internal class SearchTestSpy : MonoBehaviour, ISearchable
    {
        public bool IsCalled { get; private set; } = false;

        public void OnSearch()
        {
            IsCalled = true;
        }

    }
    public class SearcherTestSpy : Searcher
    {
        public GameObject searchable;
        protected override GameObject[] Recognize()
        {
            return new GameObject[] { searchable };
        }
    }

    public class SearcherUnitTest
    {

        Searcher searcher;
        GameObject searcherObject;
        GameObject target;
        SearchTestSpy searchTestSpy;
        /// <summary>
        /// テストシーンの作成
        /// </summary>
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            target = new();
            target.transform.position = 2.0f * Vector3.forward;
            SphereCollider collider = target.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = 0.5f;
            searchTestSpy = target.AddComponent<SearchTestSpy>();
            searcherObject = new();
            searcher = new SearcherTestSpy() { searchable = target };
        }

        [Test]
        public void Searcher_Search_ISearchableを発見するとOnSearchを実行する()
        {
            searcher.Search();
            Assert.That(searchTestSpy.IsCalled, Is.True);
        }
    }
}