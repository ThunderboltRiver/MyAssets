using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using ItemSearchSystem;
using UnityEngine.TestTools;
using System.Linq;

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
            taker = new();
            var takableAddPresenter = presenter.AddComponent<TakableAddPresenter>();
            takableAddPresenter.ChangeTaker(taker);
            takableAddPresenter.ChangeView(view);

        }

        [UnityTest]
        public IEnumerator TakableAddPresenter_選択中のオブジェクトの追加に対して対応のViewが作成されるか()
        {
            GameObject[] selections = { new GameObject() };
            selections[0].AddComponent<TakableTestSpy>();
            taker.Select(selections);
            yield return null;
            Assert.That(view.activeSelf, Is.True);
        }

        [UnityTest]
        public IEnumerator TakableAddPresenter_選択中のオブジェクトの減少に対して対応のViewが非表示されるか()
        {
            GameObject[] selections = { new GameObject(), new GameObject() };
            selections[0].AddComponent<TakableTestSpy>();
            selections[1].AddComponent<TakableTestSpy>();
            taker.Select(selections);
            bool isActivated = view.activeSelf;
            yield return null;
            taker.Select(new GameObject[] { selections[0] });
            yield return null;
            Assert.That((isActivated ^ view.activeSelf) && !view.activeSelf, Is.True);
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