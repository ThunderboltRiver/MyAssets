using System;
using NUnit.Framework;
using ItemSearchSystem;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace EditModeTests
{
    public class ItemInventryIntegrationTest
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
            _ = target.AddComponent<STRTestSpy>();
        }

        [Test]
        public void Searcher_SearchがTrueのときTakableStackMaskを超えてなければTaker_TryPushTakableがTrue()
        {
            GameObject player = new();
            Searcher searcher = new(player.transform, 0.5f, 1.0f);
            Taker taker = new() { TakableStackMask = 1 };
            Assert.That(searcher.Search(out GameObject gameObject) && taker.TryPushTakable(gameObject), Is.True);
        }
        [Test]
        public void Searcher_SearchがTrueのときTakableStackMaskを超えるとTaker_TryPushTakableができない()
        {

            GameObject player = new();
            Searcher searcher = new(player.transform, 0.5f, 1.0f);
            Taker taker = new() { TakableStackMask = 1 };
            Assert.That(searcher.Search(out GameObject gameObject) && taker.TryPushTakable(gameObject) && !taker.TryPushTakable(gameObject), Is.True);
        }

        [Test]
        public void Taker_TakeしたオブジェクトがIRegistableならRegister_TryRegistを行う()
        {
            Taker taker = new() { TakableStackMask = 1 };
            Register register = new();
            taker.TryPushTakable(target);
            Assert.That(taker.Take(out object obj) && register.TryRegist(obj), Is.True);
        }
    }

    internal class STRTestSpy : MonoBehaviour, ISearchable, ITakable, IRegistable
    {
        public int MaxRegistalbe { get; } = 1;
        public bool IsOnSearchCalled { get; private set; } = false;
        public bool IsOnTakenCalled { get; private set; } = false;
        public bool IsOnRegistCalled { get; private set; } = false;
        public void OnSearch()
        {
            IsOnSearchCalled = true;
        }

        public void OnTaken()
        {
            IsOnTakenCalled = true;
        }

        public void OnRegist()
        {
            IsOnRegistCalled = true;
        }
    }
}