using NUnit.Framework;
using UnityEngine;
using ItemSearchSystem;
using ObservableCollections;
using System.Collections.Specialized;
using UnityEditor.SceneManagement;
using System.Runtime.InteropServices;
using UniRx;

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

        [Test]
        public void ItemSearchSystemManager_SearchAndTryPushTakable_何も検索していないときはTaker_Takeができない()
        {
            _ = target.AddComponent<STRTestSpy>();
            GameObject player = new();
            Searcher searcher = new(player.transform, 0.5f, 1.0f);
            Taker taker = new() { TakableStackMask = 1 };
            Register register = new();
            ItemSearchSystemManager manager = new(searcher, taker, register);
            manager.SearchAndTryPushTakable();
            player.transform.Rotate(60 * Vector3.up);
            manager.SearchAndTryPushTakable();
            Assert.That(taker.Take(out object _), Is.False);
        }

        [Test]
        public void ItemSearchSystemManager_WaitingTakablesChangedAsObservable_待機中のITakableのコレクションの増大が通知される()
        {
            _ = target.AddComponent<STRTestSpy>();
            GameObject player = new();
            Searcher searcher = new(player.transform, 0.5f, 1.0f);
            Taker taker = new() { TakableStackMask = 1 };
            Register register = new();
            ItemSearchSystemManager manager = new(searcher, taker, register);
            bool IsPushedTakable = false;
            // manager.WaitingTakablesChangedAsObservable.Subscribe(
            //     args =>
            //     {
            //         IsPushedTakable = args.Action == NotifyCollectionChangedAction.Add;
            //     }
            // ).AddTo(player);
            manager.WaitingTakablesAddAsObservable
                .Subscribe(_ =>
                {
                    IsPushedTakable = true;
                })
                .AddTo(player);
            manager.SearchAndTryPushTakable();
            Assert.That(IsPushedTakable, Is.True);
        }
    }
}