using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using ItemSearchSystem;

namespace PlayModeTests
{
    [TestFixture]
    public class ISearchableUnitTest
    {
        // 環境の構築
        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("Scenes/TestScenes/SearchableTestScene");
            // テストパラメータの設定
        }

        // 検索機能のテスト
        [UnityTest]
        public IEnumerator OnSearchが実行されるか()
        {
            if (GameObject.FindObjectOfType(typeof(SearchableTestItem)) is ISearchable searchableItem)
            {
                searchableItem.OnSearch();
            };
            yield return null;
            LogAssert.Expect(LogType.Log, "Searched This Item");
        }
    }


}

