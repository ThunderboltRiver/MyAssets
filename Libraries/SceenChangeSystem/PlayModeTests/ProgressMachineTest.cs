using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;
using ScreenChangeSystem;

namespace ScreenChangeSystemTests
{
    public class GameProgressTest : GameProgress
    {

    }

    public class ProgressMachineTest
    {
        // A Test behaves as an ordinary method
        enum ProgressKey
        {
            Start,
            Game,
            Result,
        }

        private ProgressMachine progressMachine;

        [SetUp]
        public void SetUp()
        {
            GameObject gameObject = new();
            progressMachine = gameObject.AddComponent<ProgressMachine>();
            var gameProgress = new GameProgressTest();
            progressMachine.AddStartProgress((int)ProgressKey.Start, gameProgress);
        }

        [Test]
        public void ProgressMachine_ChangeGameProgress_現在のGameProgressを変更できる()
        {
            progressMachine.ChangeGameProgress((int)ProgressKey.Start);
            string gameProgressName = "";
            progressMachine.CurrentGameProgress.Subscribe(value =>
            {
                gameProgressName = value;
            });
            Assert.That(gameProgressName, Is.EqualTo(typeof(GameProgressTest)));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ProgressMachineTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}