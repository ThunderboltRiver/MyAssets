using UnityEngine;
using NUnit.Framework;
using ItemSearchSystem;
using UnityEditor.SceneManagement;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using System;

namespace EditModeTests
{
    internal class TakableTestSpy : MonoBehaviour, ITakable
    {
        public bool IsCalled { get; private set; }
        public bool IsCalledOnSelected { get; private set; }
        public bool IsCalledOnDeselected { get; private set; }
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
            IsCalledOnDeselected = true;
        }
    }

    public class TakerUnitTest
    {
        GameObject takableObject;
        TakableTestSpy takable;
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            takableObject = new();
            takableObject.transform.position = Vector3.forward;
            takable = takableObject.AddComponent<TakableTestSpy>();
            _ = takableObject.AddComponent<SphereCollider>();
        }
        [Test]
        public void Taker_CurrentSelectionsはReadOnlyReactiveCollectionである()
        {
            Taker taker = new();
            Assert.That(taker.CurrentSelections is IReadOnlyReactiveCollection<GameObject>, Is.True);
        }

        [Test]
        public void Taker_Select_maxSelectionを超えてゲームオブジェクトを選択できない()
        {
            int maxSelection = 2;
            Taker taker = new(maxSelection);
            GameObject[] overInputs = { new GameObject(), new GameObject(), new GameObject() };
            Array.ForEach(overInputs, gameObject => { gameObject.AddComponent<TakableTestSpy>(); });
            taker.Select(overInputs);
            Assert.That(taker.CurrentSelections.Count, Is.LessThanOrEqualTo(taker.maxSelection));
        }

        [Test]
        public void Taker_Select_要素が増えるとCurrentSelectionsから要素の追加の通知を受け取ることができる()
        {
            Taker taker = new();
            GameObject gameObject = new();
            gameObject.AddComponent<TakableTestSpy>();
            bool IsCalled = false;
            taker.CurrentSelections.ObserveAdd().Subscribe(addevent =>
            {
                IsCalled = true;
            })
            .AddTo(gameObject)
            ;
            taker.Select(new GameObject[] { gameObject });
            Assert.That(IsCalled, Is.True);
        }

        [Test]
        public void Taker_Select_要素が減るとCurrentSelectionsから要素の削除の通知を受け取ることができる()
        {
            Taker taker = new();
            GameObject gameObject = new();
            gameObject.AddComponent<TakableTestSpy>();
            bool IsCalled = false;
            taker.CurrentSelections.ObserveRemove().Subscribe(addevent =>
            {
                IsCalled = true;
            })
            .AddTo(gameObject)
            ;
            taker.Select(new GameObject[] { gameObject });
            taker.Select(new GameObject[] { });
            Assert.That(IsCalled, Is.True);
        }
        [Test]
        public void Taker_Select_出力値の配列が入力値の配列の部分集合であるか()
        {
            GameObject gameObject1 = new();
            GameObject gameObject2 = new();
            gameObject1.AddComponent<TakableTestSpy>();
            gameObject2.AddComponent<TakableTestSpy>();
            GameObject[] inputed = { gameObject1, gameObject2 };
            Taker taker = new();
            Assert.That(taker.Select(inputed), Is.SubsetOf(inputed));
        }
        [Test]
        public void Taker_Select_出力する前に出力値のオブジェクトのOnSelectedを呼び出す()
        {
            GameObject gameObject = new();
            takable = gameObject.AddComponent<TakableTestSpy>();
            GameObject[] inputed = { gameObject };
            Taker taker = new();
            taker.Select(inputed);
            Assert.That(takable.IsCalledOnSelected, Is.True);
        }
        [Test]
        public void Taekr_Deselect_出力前にOnDeselectedを呼び出す()
        {
            Taker taker = new();
            GameObject gameObject = new();
            takable = gameObject.AddComponent<TakableTestSpy>();
            GameObject[] inputed = { gameObject };
            taker.Select(inputed);
            taker.Deselect();
            Assert.That(takable.IsCalledOnDeselected, Is.True);
        }
        [Test]
        public void Taekr_以前のSelectionsにはあるが現在のSelectionsにはない分のGameObjectは自動でDeselectされる()
        {
            Taker taker = new();
            GameObject[] oldGameObjects = { takableObject };
            taker.Select(oldGameObjects);
            GameObject[] nextGameObjects = { new GameObject() };
            taker.Select(nextGameObjects);
            Assert.That(takable.IsCalledOnDeselected, Is.True);
        }
        [Test]
        public void Taker_Select_出力結果がCurrentSelectionプロパティと一致する()
        {
            GameObject gameObject1 = new();
            GameObject gameObject2 = new();
            gameObject1.AddComponent<TakableTestSpy>();
            gameObject2.AddComponent<TakableTestSpy>();
            GameObject[] inputed = { gameObject1, gameObject2 };
            Taker taker = new();
            Assert.That(taker.Select(inputed), Is.EqualTo(taker.CurrentSelections));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Taker_Take_入力インデックスに対応した要素がない場合は出力はfalse(int index)
        {
            Taker taker = new();
            Assert.That(taker.Take(index, out object _), Is.False);
        }

        [Test]
        public void Taker_Take_成功したゲームオブジェクトはCurrentSelectionsから削除される()
        {

            Taker taker = new();
            GameObject[] selections = { takableObject };
            taker.Select(selections);
            taker.Take(0, out object _);
            Assert.That(taker.CurrentSelections, Has.No.Member(takableObject));
        }

        [Test]
        public void 検証_プロパティの古い値を保持できるか()
        {
            Taker taker = new();
            GameObject gameObject1 = new();
            GameObject gameObject2 = new();
            gameObject1.AddComponent<TakableTestSpy>();
            gameObject2.AddComponent<TakableTestSpy>();
            GameObject[] oldinputed = { gameObject1 };
            taker.Select(oldinputed);
            IEnumerable<GameObject> oldSelections = taker.CurrentSelections.ToArray();
            GameObject[] newinputed = { gameObject2 };
            taker.Select(newinputed);
            Assert.That(oldSelections, Is.Not.EqualTo(taker.CurrentSelections.ToArray()));
        }
    }
}