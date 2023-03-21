using System;
using UnityEngine;
using ObservableCollections;
using System.Collections.Specialized;
using NUnit.Framework;
using ItemSearchSystem;
using NSubstitute;
using UnityEditor.SceneManagement;
using NUnit.Framework.Internal;
using NSubstitute.ExceptionExtensions;

namespace EditModeTests
{
    internal class TakableTestSpy : MonoBehaviour, ITakable
    {
        public bool IsCalled { get; private set; }
        public void OnTaken(Vector3 takeDirection)
        {
            IsCalled = true;
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
            takable = takableObject.AddComponent<TakableTestSpy>();

        }

        [TestCase(2)]
        public void Taker_TryPushTakable_TakableStackMask以内の個数のITakeableオブジェクトを格納できる(int takableStackMask)
        {
            Taker taker = new() { TakableStackMask = takableStackMask };

            Assert.That(taker.TryPushTakable(takableObject)
                && taker.TryPushTakable(Substitute.For<ITakable>())
                && !taker.TryPushTakable(Substitute.For<ITakable>())
                , Is.True);
        }

        [Test]
        public void Taker_Take_格納したITakableオブジェクトのOnTakenを実行している()
        {
            Taker taker = new() { TakableStackMask = 1 };
            taker.TryPushTakable(takable);
            Assert.That(taker.Take(out object _) && takable.IsCalled, Is.True);
        }

        [Test]
        public void Taker_Take_実行後にITakableオブジェクトを所持していない()
        {
            Taker taker = new() { TakableStackMask = 1 };
            taker.TryPushTakable(takable);
            Assert.That(taker.Take(out object _) && !taker.HasTakableObject(takableObject), Is.True);
        }

        [TestCase(-1)]
        public void Taker_TakableStackMask_負の値は取らないか(int testCase)
        {
            Taker taker = new() { TakableStackMask = testCase };
            Assert.That(taker.TakableStackMask, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void Taker_ClearTakable_格納されたTakableをクリアできる()
        {
            Taker taker = new() { TakableStackMask = 1 };
            taker.TryPushTakable(takableObject);
            taker.ClearTakable();
            Assert.That(taker.TakableCount, Is.EqualTo(0));
        }

        [Test]
        public void Taker_TakableStackの増加の通知を取得できる()
        {
            Taker taker = new() { TakableStackMask = 1 };
            bool IsPushedTakable = false;
            taker.WaitingTakablesAsObservableCollection.CollectionChanged += (in NotifyCollectionChangedEventArgs<ITakable> args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    IsPushedTakable = true;
                    return;
                }
            };
            taker.TryPushTakable(takableObject);
            Assert.That(IsPushedTakable, Is.True);
        }
        [Test]
        public void Taker_Take_引数としてTakeするVector3型の方向を指定できる()
        {
            Taker taker = new() { TakableStackMask = 1 };
            Assert.That(() => taker.Take(Vector3.forward, out _), Throws.Nothing);
        }

    }
}