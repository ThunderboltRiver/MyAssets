using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MainGameScene.Model;

namespace EditModeTests
{
    [TestFixture]
    public class DebugItemTest
    {
        DebugItem debugItem;
        // 環境の構築
        [SetUp]
        public void Setup()
        {
            // テストパラメータの設定
            debugItem = ScriptableObject.CreateInstance<DebugItem>();
        }

        // 検索機能のテスト
        [Test]
        public void TestOnSearch()
        {
            // 検索を期待する
            LogAssert.Expect(LogType.Log, "Searched DebugItem");
            // DebugItemを発見したとき
            debugItem.OnSearch();

        }

        // 選択機能のテスト
        [Test]
        public void TestOnSelect()
        {
            // 選択を期待する
            LogAssert.Expect(LogType.Log, "Selected This DebugItem");
            // DebugItemが選択されたとき
            debugItem.OnSelect();
        }

        // 使用機能のテスト
        [Test]
        public void TestUse()
        {
            // 使用を期待する
            LogAssert.Expect(LogType.Log, "Called UseMethod in This DebugItem");
            // DebugItemを使用する
            debugItem.Use();
        }
    }


}

