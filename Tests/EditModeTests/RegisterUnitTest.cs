using System.Collections.Generic;
using System.Linq;
using ItemSearchSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EditModeTests
{
    public class RegisterUnitTest
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void Register_TryRegist_RegistableMonoBehaviourMockをGameObjectから登録できる()
        {
            Register register = new();
            GameObject gameObject = new();
            _ = gameObject.AddComponent<RegistableMonoBehaviourMock>();
            Assert.That(register.TryRegist(gameObject), Is.True);
        }

        [Test]
        public void Register_TryRegist_RegistableMockを登録できる()
        {
            Register register = new();
            IRegistable registable = new RegistableMock();
            Assert.That(register.TryRegist(registable), Is.True);
        }

        [Test]
        public void Register_TryRegist__RegistableMockの所持可能数を超えるときはRegistableMockの登録はできない()
        {
            Register register = new();
            for (int i = 0; i < 2; i++)
            {
                register.TryRegist(new RegistableMock());

            }
            Assert.That(register.TryRegist(new RegistableMock()), Is.False);
        }

        [Test]
        public void Register_TryRegist_ただのGameObjectは登録できない()
        {
            Register register = new();
            GameObject unRegistable = new();
            Assert.That(register.TryRegist(unRegistable), Is.False);
        }

        [Test]
        public void Register_TryRegist_RegistableMockの登録完了時にRegistableMockがDebugLogに出力される()
        {
            Register register = new();
            IRegistable registable = new RegistableMock();
            LogAssert.Expect(LogType.Log, "RegistableMock");
            register.TryRegist(registable);
        }

        [Test]
        public void Register_GetAllRegistered_すべての登録済みデータを取得する()
        {
            Register register = new();
            IRegistable registable1 = new RegistableMock();
            register.TryRegist(registable1);
            IRegistable registable2 = Factory.CreateComponent<RegistableMonoBehaviourMock>();
            register.TryRegist(registable2);
            IEnumerable<(IRegistable, int)> expect = new List<(IRegistable, int)>() { (registable1, 1), (registable2, 1) };
            Assert.That(register.GetAllRegistered(), Is.EqualTo(expect));
        }

    }

    class RegistableMock : IRegistable
    {
        public int MaxRegistalbe => 2;
        public void OnRegist()
        {
            Debug.Log("RegistableMock");
        }

        public override string ToString()
        {
            return typeof(RegistableMock).ToString();
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is RegistableMock;
        }
    }

    class RegistableMonoBehaviourMock : MonoBehaviour, IRegistable
    {
        public int MaxRegistalbe => 1;
        public void OnRegist()
        {

        }
    }

    static class Factory
    {
        public static T Create<T>() where T : new()
        {
            return new T();
        }

        public static T CreateComponent<T>() where T : MonoBehaviour
        {
            return Create<GameObject>().AddComponent<T>();

        }

    }
}