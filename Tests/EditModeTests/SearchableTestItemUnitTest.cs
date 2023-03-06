using System.Runtime.Serialization;
using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ItemSearchSystem;


namespace EditModeTests
{
    public class SearchableTestItemUnitTest
    {
        ISearchable searchable;
        readonly GameObject gameObjectSearchable = new();
        readonly GameObject gameObjectNotSearchable = new();

        [SetUp]
        public void Setup()
        {
            _ = gameObjectSearchable.AddComponent<SearchableTestItem>();
        }

        [Test]
        public void GameObjectからISearchableを取得できるか()
        {
            Assert.False(gameObjectNotSearchable.TryGetComponent(out searchable));
            Assert.True(gameObjectSearchable.TryGetComponent(out searchable));
        }

        [Test]
        public void ISearchableのOnSearchを呼び出せるか()
        {
            if (gameObjectSearchable.TryGetComponent(out searchable))
            {
                LogAssert.Expect(LogType.Log, "This item was Searched");
                searchable.OnSearch();
            }

        }
    }
}