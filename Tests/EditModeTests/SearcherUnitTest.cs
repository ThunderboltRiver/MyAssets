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
            float radius = 0.5f;
            float maxDistance = 1.0f;
            searcherObject = new();
            searcher = new(searcherObject.transform, radius, maxDistance);
        }

        [Test]
        public void Searcher_Search_検索方向が登録されたtransform_forwardと一致する()
        {
            searcherObject.transform.Rotate(30 * Vector3.up);
            Assert.That(searcher.SearchDirection, Is.EqualTo(searcherObject.transform.forward));
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
            searcher = new();
            Assert.That(searcher.Search(out GameObject _), Is.False);
        }

        [Test]
        public void Searcher_Constracter_他のSearcherインスタンスから異なるインスタンスを作成する()
        {
            Searcher searcher1, searcher2;
            searcher1 = new();
            searcher2 = new(searcher1.Transform, 2 * searcher1.Radius, 2 * searcher1.MaxDistance);
            Assert.That(searcher1, Is.Not.EqualTo(searcher2));
        }

        [Test]
        public void 検証_参照型のgetは参照渡しになるか()
        {
            GameObject gameObject = new();
            Searcher searcher1 = new(gameObject.transform, 0.5f, 1.0f);
            Assert.That(searcher1.Transform, Is.EqualTo(gameObject.transform));
        }
        [Test]
        public void 検証_参照型のgetを受け取ると外部から変更が可能になるか()
        {
            GameObject gameObject = new();
            Searcher searcher1 = new(gameObject.transform, 0.5f, 1.0f);
            searcher1.Transform.position = Vector3.up;
            Assert.That(searcher1.Transform.position, Is.EqualTo(Vector3.up));
        }

    }
}