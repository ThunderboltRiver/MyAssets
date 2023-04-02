using System.Collections.Generic;
using ItemSearchSystem;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using NSubstitute;

namespace EditModeTests
{
    public class RegisterUnitTest
    {
        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

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
        public void Register_TryRegist_IRegistableオブジェクトなら登録可能でそれ以外は不可()
        {
            Register register = new();
            object registable = SubstituteIRegistalbe(1);
            object unRegistable = new();
            Assert.That(register.TryRegist(registable) && !register.TryRegist(unRegistable), Is.True);
        }

        [Test]
        public void Register_TryRegist_IRegistableのMaxRegistableを超えるときは同じIRegistableオブジェクトの登録はできない()
        {
            Register register = new();
            IRegistable registable = Substitute.For<IRegistable>();
            registable.MaxRegistalbe.Returns(2);
            Assert.That(register.TryRegist(registable) && register.TryRegist(registable) && !register.TryRegist(registable), Is.True);
        }

        [Test]
        public void Register_TryRegist_ただのGameObjectは登録できない()
        {
            Register register = new();
            GameObject unRegistable = new();
            Assert.That(register.TryRegist(unRegistable), Is.False);
        }

        [Test]
        public void Register_TryRegist_RegistableMockの登録完了時にIRegistable_OnRegsitが呼ばれる()
        {
            Register register = new();
            IRegistable registable = SubstituteIRegistalbe(1);
            bool isCalled = false;
            registable.When(x => x.OnRegist(register.Owner)).Do(_ => isCalled = true);
            Assert.That(register.TryRegist(registable) && isCalled, Is.True);
        }

        [Test]
        public void Register_GetAllRegistered_すべての登録済みデータを取得する()
        {
            Register register = new();
            IRegistable registable1 = SubstituteIRegistalbe(1);
            IRegistable registable2 = SubstituteIRegistalbe(1);
            register.TryRegist(registable1);
            register.TryRegist(registable2);
            IEnumerable<(IRegistable, int)> expect = new List<(IRegistable, int)>() { (registable1, 1), (registable2, 1) };
            Assert.That(register.GetAllRegistered(), Is.EqualTo(expect));
        }

        private IRegistable SubstituteIRegistalbe(int maxRegistalbe)
        {
            IRegistable registable = Substitute.For<IRegistable>();
            registable.MaxRegistalbe.Returns(maxRegistalbe);
            return registable;
        }

    }
    internal class RegistableMonoBehaviourMock : MonoBehaviour, IRegistable
    {
        public int MaxRegistalbe => 1;
        public void OnRegist(GameObject owner)
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