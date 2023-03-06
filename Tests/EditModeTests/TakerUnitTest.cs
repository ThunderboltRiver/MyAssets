using UnityEngine;
using NUnit.Framework;
using ItemSearchSystem;
using UnityEngine.TestTools;

namespace EditModeTests
{
    public class TakerUnitTest
    {
        readonly GameObject takableObject = new();
        ITakable takable;
        [SetUp]
        public void Setup()
        {
            takable = takableObject.AddComponent<TakableMock>();

        }

        [Test]
        public void Takeableオブジェクトを一時的に格納できるか()
        {
            Taker taker = new()
            {
                _takableStackMask = 2
            };
            Assert.True(taker.TryPushTakable(takable));
            Assert.True(taker.TryPushTakable(takableObject));
        }

        [Test]
        public void 格納されたTakableオブジェクトを実行できる()
        {
            Taker taker = new()
            {
                _takableStackMask = 1
            };
            Assert.True(taker.TryPushTakable(takable));
            LogAssert.Expect(LogType.Log, "TakableMock was Taken");
            taker.Take();
        }

        [Test]
        public void Take実行後にTakableオブジェクトが開放されているか()
        {
            Taker taker = new()
            {
                _takableStackMask = 1
            };
            Assert.True(taker.TryPushTakable(takable));
            taker.Take();
            Assert.False(taker.HasTakableObject(takableObject));
        }

    }
}