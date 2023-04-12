using NUnit.Framework;

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
    }
}