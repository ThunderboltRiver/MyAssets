using NUnit.Framework;
using UnityEngine;
using NSubstitute;
using ItemSearchSystem;
using UnityEditor.SceneManagement;

namespace EditModeTests
{
    public class SearchSystemManagerUnitTest
    {
        GameObject target;
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            target = new();
            target.transform.position = 2.0f * Vector3.forward;
            SphereCollider collider = target.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = 0.5f;
        }

        [Test]
        public void ItemSearchSystemManager_SearchAndTryPushTakable_ISearchableかつITakableならTrue()
        {
            STRTestSpy testSpy = target.AddComponent<STRTestSpy>();
            GameObject player = new();
            Searcher searcher = new(player.transform, 0.5f, 1.0f);
            Taker taker = new() { TakableStackMask = 1 };
            Register register = new();
            ItemSearchSystemManager manager = new(searcher, taker, register);
            Assert.That(manager.SearchAndTryPushTakable(), Is.True);
        }

        [Test]
        public void ItemSearchSystemManager_TakeAndTryRegist_ITakableがスタックにありさらにそれがIRegistalbeならTrue()
        {
            STRTestSpy testSpy = target.AddComponent<STRTestSpy>();
            GameObject player = new();
            Searcher searcher = new(player.transform, 0.5f, 1.0f);
            Taker taker = new() { TakableStackMask = 1 };
            Register register = new();
            ItemSearchSystemManager manager = new(searcher, taker, register);
            manager.SearchAndTryPushTakable();
            Assert.That(manager.TakeAndTryRegist(), Is.True);
        }

    }
}