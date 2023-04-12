using NUnit.Framework;
using State = StateMachine.StateMachine<object>.State;
namespace StateMachine
{
    public class StateMachineUnitTest
    {
        // A Test behaves as an ordinary method
        public class State1 : State
        {
        }

        public class State2 : State
        {
        }
        public class State3 : State
        {
        }


        [Test]
        public void StateMachine_State1で開始するとeventKeyが1を発行されたときにState2に遷移する()
        {
            object player = new();
            var stateMachine = new StateMachine<object>(player);
            var eventKey = 1;
            stateMachine.AddTransition<State1, State2>(eventKey);
            stateMachine.Start<State1>();
            bool didStartState1 = stateMachine.CurrentState is State1;
            stateMachine.DispatchEvent(eventKey);
            bool didEndState2 = stateMachine.CurrentState is State2;
            Assert.That(didStartState1 && didEndState2, Is.True);
        }

        [Test]
        public void StateMachine_State1で開始するとeventKeyが2を発行されたときにState2に遷移しない()
        {
            object player = new();
            var stateMachine = new StateMachine<object>(player);
            var eventKey = 1;
            stateMachine.AddTransition<State1, State2>(eventKey);
            stateMachine.Start<State1>();
            bool didStartState1 = stateMachine.CurrentState is State1;
            stateMachine.DispatchEvent(2);
            bool didEndState2 = stateMachine.CurrentState is State2;
            Assert.That(didStartState1 && didEndState2, Is.False);
        }
        [Test]
        public void StateMachine_AddTransition_すでにState1から同じeventKeyでの遷移が登録されている場合は新しい遷移で上書きされる()
        {
            object player = new();
            var stateMachine = new StateMachine<object>(player);
            var eventKey = 1;
            stateMachine.AddTransition<State1, State2>(eventKey);
            bool isBeforeState2 = stateMachine.CurrentState == new State2();
            stateMachine.AddTransition<State1, State3>(eventKey);
            bool isAfterState3 = stateMachine.CurrentState == new State3();
            Assert.That(isBeforeState2 && isAfterState3, Is.True);
        }

        [Test]
        public void StateMachine_Start_State1で開始するとState1になる()
        {
            object player = new();
            var stateMachine = new StateMachine<object>(player);
            stateMachine.Start<State1>();
            Assert.That(stateMachine.CurrentState, Is.EqualTo(new State1()));
        }
    }
}