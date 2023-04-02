using ItemSearchSystem;
using UnityEngine;
using UnityEditor.SceneManagement;
using NUnit.Framework;

namespace EditModeTests
{
    internal class SearchTestSpy : MonoBehaviour, ISearchable
    {
        public bool IsCalled { get; private set; } = false;

        public bool IsOnDesearchCalled { get; private set; } = false;

        public void OnSearch()
        {
            IsCalled = true;
        }

        public void OnDesearch()
        {
            IsOnDesearchCalled = true;
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
            // target.transform.position = 2.0f * Vector3.forward;
            // SphereCollider collider = target.AddComponent<SphereCollider>();
            // collider.center = Vector3.zero;
            // collider.radius = 0.5f;
            searchTestSpy = target.AddComponent<SearchTestSpy>();
            searcher = new SearcherTestSpy() { searchable = target };
        }

        [Test]
        public void Searcher_Search_ISearchableを発見するとOnSearchを実行する()
        {
            searcher.Search();
            Assert.That(searchTestSpy.IsCalled, Is.True);
        }

        [Test]
        public void Searcher_Desearch_要素が減るとその要素のOnDesearchが呼ばれる()
        {
            searcher.Search();
            searcher.Desearch((GameObject gameObject) => gameObject == target);
            Assert.That(searchTestSpy.IsOnDesearchCalled, Is.True);
        }
    }
}