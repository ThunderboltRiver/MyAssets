using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using ItemSearchSystem;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace PlayModeTests
{
    public class TakableAddPresenterUnitTest
    {
        [SetUp]
        public void Setup()
        {
            var scene = SceneManager.CreateScene("New Scene");
            SceneManager.SetActiveScene(scene);
        }

        [UnityTest]
        public IEnumerator TakableAddPresenter_CreateView_takables変更時に対応のViewが作成されるか()
        {
            GameObject presenter = new();
            GameObject view = new();
            view.SetActive(false);
            Taker taker = new() { TakableStackMask = 1 };
            var takableAddPresenter = presenter.AddComponent<TakableAddPresenter>();
            takableAddPresenter.ChangeTaker(taker);
            takableAddPresenter.ChangeView(view);
            yield return null;
            taker.TryPushTakable(new GameObject().AddComponent<TakableTestSpy>());
            Assert.That(view.activeSelf, Is.True);
        }
    }
    internal class TakableTestSpy : MonoBehaviour, ITakable
    {
        public bool IsCalled { get; private set; }
        public void OnTaken()
        {
            IsCalled = true;
        }

    }
}