using UnityEngine;
using NUnit.Framework;
using ItemSearchSystem;
using NSubstitute;
using UnityEditor.SceneManagement;

namespace EditModeTests
{
    internal class TakableTestSpy : MonoBehaviour, ITakable
    {
        public bool IsCalled { get; private set; }
        public void OnTaken()
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

    }
}