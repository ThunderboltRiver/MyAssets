using UnityEngine;
using NUnit.Framework;
using ItemSearchSystem;
using NSubstitute;
using UnityEditor.SceneManagement;
using NUnit.Framework.Internal;


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
            takableObject.transform.position = Vector3.forward;
            takable = takableObject.AddComponent<TakableTestSpy>();
            _ = takableObject.AddComponent<SphereCollider>();
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

        [TestCase(-1)]
        public void Taker_TakableStackMask_負の値は取らないか(int testCase)
        {
            Taker taker = new() { TakableStackMask = testCase };
            Assert.That(taker.TakableStackMask, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void Taker_Take_Takerを始点とする位置ベクトルにあるTakableオブジェクトのonTakeを実行する()
        {
            GameObject player = new();
            Taker taker = new(player, 1.5f) { TakableStackMask = 1 };
            Vector3 rerativePosition = 2 * player.transform.forward;
            taker.Take(rerativePosition, out _);
            Assert.That(takable.IsCalled, Is.True);
        }

    }
}