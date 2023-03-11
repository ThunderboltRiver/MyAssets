using ItemSearchSystem;
using UnityEngine;
using UnityEditor.SceneManagement;
using NUnit.Framework;

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

    public class SearcherUnitTest
    {

        Searcher searcher = new();
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

        }

        [Test]
        public void Searcher_Search_ISearchableなGameObjectを発見したらTrue()
        {
            Assert.That(searcher.Search(out GameObject gameObject) && gameObject.TryGetComponent(out ISearchable _), Is.True);
        }
        [Test]
        public void Searcher_Search_ISearchableを発見するとOnSearchを実行する()
        {
            Assert.That(searcher.Search(out GameObject _) && searchTestSpy.IsCalled, Is.True);
        }

        [Test]
        public void Searcher_Search_ISearchableがなければFalse()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            Assert.That(searcher.Search(out GameObject _), Is.False);
        }

    }
}