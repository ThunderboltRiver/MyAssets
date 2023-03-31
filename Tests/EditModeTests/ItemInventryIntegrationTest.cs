using System;
using NUnit.Framework;
using ItemSearchSystem;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Linq;
using MainGameScene.Model;
using UniRx;

namespace EditModeTests
{
    public class ItemInventryIntegrationTest
    {
        GameObject target;
        STRTestSpy testSpy;
        const float SEARCHING_RADIUS = 2.0f;
        const float SERECTING_LENGTH = 1.5f;

        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            target = new();
            target.transform.position = SEARCHING_RADIUS * Vector3.forward;
            SphereCollider collider = target.AddComponent<SphereCollider>();
            collider.center = Vector3.zero;
            collider.radius = SEARCHING_RADIUS - SERECTING_LENGTH;
            testSpy = target.AddComponent<STRTestSpy>();
        }

        [Test]
        public void STRTestSpyを持ったゲームオブジェクトをSearchしSelect状態にできる()
        {
            GameObject player = new();
            Searcher searcher = new SphereSearcher(player.transform, SEARCHING_RADIUS); // プレイヤーを中心に指定した半径以内にあるISearchableを取得するクラス
            Taker taker = new RayCastingTaker(player.transform, SERECTING_LENGTH); // レイキャストして当たったオブジェクトのみをSelectの対象にするTaker
            searcher.Search();
            taker.Select(searcher.CurrentRecognition.ToArray());
            Assert.That(taker.CurrentSelections.ToArray(), Is.EqualTo(new GameObject[] { target }));
        }
        [Test]
        public void ITakableなゲームオブジェクトがSelectされたときの通知を取得できる()
        {
            GameObject player = new();
            Taker taker = new RayCastingTaker(player.transform, SERECTING_LENGTH); // レイキャストして当たったオブジェクトのみをSelectの対象にするTaker
            bool IsAddEventCatch = false;
            taker.CurrentSelections.ObserveAdd().Subscribe(
                addEvent =>
                {
                    IsAddEventCatch = true;
                }
            ).AddTo(target);
            taker.Select(new GameObject[] { target });
            Assert.That(IsAddEventCatch, Is.True);
        }

        [Test]
        public void 選択中のゲームオブジェクトの中から一つ指定してTakeしIRegistableならインベントリに追加される()
        {
            GameObject player = new();
            int index = 0;
            Taker taker = new RayCastingTaker(player.transform, SERECTING_LENGTH); // レイキャストして当たったオブジェクトのみをSelectの対象にするTaker
            taker.Select(new GameObject[] { target });
            Register register = new Register(player); //所有者が存在するRegister
            Assert.That(taker.Take(index, out GameObject takenGameObject) && register.TryRegist(takenGameObject), Is.True);
        }

        [Test]
        public void インベントリに追加されたアイテムを外部から取得できる()
        {
            Register register = new();
            GameObject registeredItem = new();
            register.Inventry.ObserveAdd()
            .Subscribe(addEvent =>
            {
                registeredItem = addEvent.Value;
            })
            .AddTo(target);
            register.TryRegist(target);
            Assert.That(registeredItem, Is.EqualTo(target));
        }

    }

    internal class STRTestSpy : MonoBehaviour, ISearchable, ITakable, IRegistable
    {
        public int MaxRegistalbe { get; } = 1;
        public bool IsOnSearchCalled { get; private set; } = false;
        public bool IsOnTakenCalled { get; private set; } = false;
        public bool IsOnSelectedCalled { get; private set; }
        public bool IsOnRegistCalled { get; private set; } = false;
        public void OnSearch()
        {
            IsOnSearchCalled = true;
        }

        public void OnTaken(Vector3 takeDirection)
        {
            IsOnTakenCalled = true;
        }
        public void OnSelected()
        {
            IsOnSearchCalled = true;
        }

        public void OnDeselected() { }

        public void OnRegist()
        {
            IsOnRegistCalled = true;
        }
    }
}