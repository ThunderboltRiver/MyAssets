using System;
using NUnit.Framework;
using State = StateMachine.StateMachine<object>.State;
namespace StateMachine
{
    public class StateMachineUnitTest
    {
        public class State1 : State
        {
            public static bool isEntered = false;
            public static bool isUpdated = false;
            public static bool isLateUpdated = false;
            public static bool isFixedUpdated = false;
            public static bool isExited = false;
            public State1()
            {
                isEntered = false;
                isUpdated = false;
                isLateUpdated = false;
                isFixedUpdated = false;
                isExited = false;
            }
            protected override void OnEnter()
            {
                isEntered = true;
            }
            protected override void OnUpdate()
            {
                isUpdated = true;
            }
            protected override void OnLateUpdate()
            {
                isLateUpdated = true;
            }
            protected override void OnFixedUpdate()
            {
                isFixedUpdated = true;
            }
            protected override void OnExit()
            {
                isExited = true;
            }
        }

        public class State2 : State
        {
        }
        public class State3 : State
        {
        }

        object player;
        StateMachine<object> stateMachine;
        int eventKey = 1;


        [SetUp]
        public void Setup()
        {
            player = new();
            stateMachine = new StateMachine<object>(player);


        }


        [Test]
        public void StateMachine_State1からState2への遷移がeventKeyが1で登録されている場合にState1で開始するとeventKeyが1を発行されたときにState2に遷移する()
        {
            stateMachine.AddTransition<State1, State2>(eventKey);
            stateMachine.Start<State1>();
            bool didStartState1 = stateMachine.CurrentState is State1;
            stateMachine.DispatchEvent(eventKey);
            bool didEndState2 = stateMachine.CurrentState is State2;
            Assert.That(didStartState1 && didEndState2, Is.True);
        }

        [Test]
        public void StateMachine_State1からState2への遷移がeventKeyが1で登録されている場合にState1で開始するとeventKeyが2を発行されたときにState2に遷移しない()
        {
            stateMachine.AddTransition<State1, State2>(eventKey);
            stateMachine.Start<State1>();
            bool didStartState1 = stateMachine.CurrentState is State1;
            stateMachine.DispatchEvent(2);
            bool didEndState2 = stateMachine.CurrentState is State2;
            Assert.That(didStartState1 && didEndState2, Is.False);
        }
        [Test]
        public void StateMachine_AddTransition_State1からState2への遷移をeventKeyを1にして登録できる()
        {
            stateMachine.AddTransition<State1, State2>(eventKey);
            stateMachine.Start<State1>();
            stateMachine.DispatchEvent(eventKey);
            Assert.That(stateMachine.CurrentState, Is.TypeOf<State2>());
        }
        [Test]
        public void StateMachine_AddTransition_すでにState1からの同じeventKeyでの遷移が登録されている場合はInvalidOperationExceptionを投げる()
        {
            stateMachine.AddTransition<State1, State2>(eventKey);
            Assert.That(() => { stateMachine.AddTransition<State1, State3>(eventKey); }, Throws.Exception.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void StateMachine_Start_State1で開始するとState1になる()
        {
            stateMachine.Start<State1>();
            Assert.That(stateMachine.CurrentState, Is.TypeOf<State1>());
        }

        [Test]
        public void StateMachine_Start_State1で開始すると内部でState1のOnEnterが呼ばれる()
        {
            stateMachine.Start<State1>();
            Assert.That(State1.isEntered, Is.True);
        }

        [Test]
        public void StateMachine_Update_CurrentStateがState1のときは内部でState1のOnUpdateが呼ばれる()
        {
            stateMachine.Start<State1>();
            stateMachine.Update();
            Assert.That(State1.isUpdated, Is.True);
        }

        [Test]
        public void StateMachine_FixedUpdate_CurrentStateがState1のときは内部でState1のOnFixedUpdateが呼ばれる()
        {
            stateMachine.Start<State1>();
            stateMachine.FixedUpdate();
            Assert.That(State1.isFixedUpdated, Is.True);
        }

        [Test]
        public void StateMachine_LateUpdate_CurrentStateがState1のときは内部でState1のOnLateUpdateが呼ばれる()
        {
            stateMachine.Start<State1>();
            stateMachine.LateUpdate();
            Assert.That(State1.isLateUpdated, Is.True);
        }

        [Test]
        public void StateMachine_DispatchEvent_CurrentStateとeventKeyに対応する遷移が登録されている場合は内部でCurrentStateのOnExitが呼ばれる()
        {
            stateMachine.Start<State1>();
            stateMachine.AddTransition<State1, State2>(eventKey);
            stateMachine.DispatchEvent(eventKey);
            Assert.That(State1.isExited, Is.True);
        }

        [Test]
        public void StateMachine_DispatchEvent_CurrentStateとeventKeyに対応する遷移が登録されている場合は内部で次のStateのOnEnterが呼ばれる()
        {
            stateMachine.Start<State2>();
            stateMachine.AddTransition<State2, State1>(eventKey);
            stateMachine.DispatchEvent(eventKey);
            Assert.That(State1.isEntered, Is.True);
        }

        [Test]
        public void StateMachine_Stop_CurrentStateを停止する()
        {
            stateMachine.Start<State1>();
            stateMachine.Stop();
            Assert.That(stateMachine.IsActive, Is.False);
        }
        [Test]
        public void StateMachine_Stop_CurrentStateを停止すると内部でCurrentStateのOnExitが呼ばれる()
        {
            stateMachine.Start<State1>();
            stateMachine.Stop();
            Assert.That(State1.isExited, Is.True);
        }

        [Test]
        public void StateMachine_DispatchEvent_CurrentStateとeventKeyに対応する遷移が登録されていない場合はCurrentStateは変わらない()
        {
            stateMachine.Start<State1>();
            stateMachine.DispatchEvent(eventKey);
            Assert.That(stateMachine.CurrentState, Is.TypeOf<State1>());
        }

    }
}