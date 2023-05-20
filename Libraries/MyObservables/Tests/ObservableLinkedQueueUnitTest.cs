using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyObservables;
using NUnit.Framework;
using UniRx;
using UnityEngine.TestTools;


namespace MyObservablesTests
{
    public class ObservableLinkedQueueUnitTest
    {
        ObservableLinkedQueue<int> _observableLinkedQueue;

        IDisposable _disposable;
        readonly int maximumCapacity = 3;
        [SetUp]
        public void Setup()
        {
            _observableLinkedQueue = new ObservableLinkedQueue<int>(maximumCapacity);
        }

        [Test]
        public void ObservableEnumerator_初期化時は値を保持しない()
        {
            var length = Enumerable.Range(0, maximumCapacity)
            .Where(i => !_observableLinkedQueue.HasValue(i))
            .Count();
            Assert.That(length, Is.EqualTo(maximumCapacity));
        }

        [Test]
        public void ObservableLinkedQueue_Enqueue_初回時は先頭だけが値を保持する()
        {
            _observableLinkedQueue.Enqueue(1);
            var resultArray = Enumerable.Range(0, maximumCapacity)
            .Where(i => _observableLinkedQueue.HasValue(i))
            .ToArray();
            Assert.That(resultArray, Is.EqualTo(new int[] { 0 }));
        }

        [Test]
        public void ObservableLinkedQueue_Enqueue_2回実行すると先頭と次の要素のみが値を持つ()
        {
            _observableLinkedQueue.Enqueue(1);
            _observableLinkedQueue.Enqueue(2);
            var resultArray = Enumerable.Range(0, maximumCapacity)
            .Where(i => _observableLinkedQueue.HasValue(i))
            .ToArray();
            Assert.That(resultArray, Is.EqualTo(new int[] { 0, 1 }));
        }

        [Test]
        public void ObservableLinkedQueue_Enqueue_maximumCapacityを超えて実行すると初回から超えた分の値が消える()
        {
            for (int i = 0; i <= maximumCapacity; i++)
            {
                _observableLinkedQueue.Enqueue(i);
            }
            var resultArray = Enumerable.Range(0, maximumCapacity)
            .Where(i => _observableLinkedQueue.HasValue(i))
            .Select(i => _observableLinkedQueue.GetCurrentValue(i))
            .ToArray();
            Assert.That(resultArray, Is.EqualTo(new int[] { 3, 2, 1 }));
        }

        [Test]
        public void ObservableLinkedQueue_Dequeue_初期化時は例外を投げる()
        {
            Assert.That(() => _observableLinkedQueue.Dequeue(), Throws.InvalidOperationException);
        }

        [Test]
        public void ObservableLinkedQueue_Dequeue_Enqueueを複数回実行した後は初回に与えた値を返す()
        {
            _observableLinkedQueue.Enqueue(1);
            _observableLinkedQueue.Enqueue(2);
            var result = _observableLinkedQueue.Dequeue();
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ObservableLinkedQueue_ValueChangeAsObservable_指定したインデックスに対応する値が変更されたことを購読できる()
        {
            int result = 0;
            _disposable = _observableLinkedQueue.ValueChangeAsObservable(0).Subscribe(x => result = x);
            _observableLinkedQueue.Enqueue(1);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void ObservableLinkedQueue_GetCurrentValue_maximumCapacityを超えて実行すると例外を投げる()
        {
            Assert.That(() => _observableLinkedQueue.GetCurrentValue(maximumCapacity), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ObservableLinkedQueue_HasValue_maximumCapacityを超えて実行すると例外を投げる()
        {
            Assert.That(() => _observableLinkedQueue.HasValue(maximumCapacity), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TearDown]
        public void TearDown()
        {
            _disposable?.Dispose();
        }

    }
}