using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using ItemSearchSystem;
using UnityEngine.TestTools;

namespace PlayModeTests
{
    public class TakableAddPresenterUnitTest
    {
        Taker taker;
        GameObject view;
        [SetUp]
        public void Setup()
        {
            GameObject presenter = new();
            view = new();
            view.SetActive(false);
            taker = new() { TakableStackMask = 1 };
            var takableAddPresenter = presenter.AddComponent<TakableAddPresenter>();
            takableAddPresenter.ChangeTaker(taker);
            takableAddPresenter.ChangeView(view);

        }

        [UnityTest]
        public IEnumerator TakableAddPresenter_CreateView_takables変更時に対応のViewが作成されるか()
        {
            taker.TryPushTakable(new GameObject().AddComponent<TakableTestSpy>());
            yield return null;
            Assert.That(view.activeSelf, Is.True);
        }

        [UnityTest]
        public IEnumerator TakableAddPresenter_Take成功後にViewが非表示になるか()
        {
            taker.TryPushTakable(new GameObject().AddComponent<TakableTestSpy>());
            yield return null;
            taker.Take(out _);
            Assert.That(view.activeSelf, Is.False);
        }
    }
    internal class TakableTestSpy : MonoBehaviour, ITakable
    {
        public bool IsCalled { get; private set; }
        public bool IsCalledOnSelected { get; private set; }
        public void OnTaken(Vector3 takeDirection)
        {
            IsCalled = true;
        }
        public void OnSelected()
        {
            IsCalledOnSelected = true;
        }
        public void OnDeselected()
        {

        }
    }

}