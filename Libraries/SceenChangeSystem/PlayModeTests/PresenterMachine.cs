using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SceenChangeSystem.Presenters;
using UniRx;

public class PresenterMachineTest
{
    // A Test behaves as an ordinary method

    private PresenterMachine<int> _presenterMachine;
    private Subject<int> _subject = new Subject<int>();

    private MockPresenter _mockPresenter;
    private MockPresenter _mockPresenter2;

    [SetUp]
    public void Setup()
    {
        _mockPresenter = new MockPresenter();
        _mockPresenter2 = new MockPresenter();
        _presenterMachine = new PresenterMachine<int>(_subject);
        _presenterMachine.BindPresenter(0, _mockPresenter);
        _presenterMachine.BindPresenter(1, _mockPresenter2);
        _presenterMachine.Activate();

    }

    [Test]
    public void PresenterMachine_observableから0が通知されると登録されたPresenterがActivate化される()
    {
        _subject.OnNext(0);
        Assert.That(_mockPresenter.IsActivated, Is.True);
    }

    [Test]
    public void PresenterMachine_observableから0が通知された後に1が通知されると登録されたPresenterがDeactivate化される()
    {
        _subject.OnNext(0);
        _subject.OnNext(1);
        Assert.That(_mockPresenter.IsDeactivated, Is.True);
    }


    [Test]
    public void PresenterMachine_Presenterが変更される前のイベントを購読可能にするObservableを購読すると0が通知される()
    {
        int result = 0;
        _presenterMachine.BeforePresenterChangeAsObservable.Subscribe(x => result = x);
        _subject.OnNext(0);
        Assert.That(result, Is.EqualTo(0));
    }


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
}
