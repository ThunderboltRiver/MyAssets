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
    }
}