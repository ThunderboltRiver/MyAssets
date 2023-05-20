using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SceenChangeSystem.Presenters;

public class CompositePresenterTest
{
    private MockPresenter _mockPresenter1;
    private MockPresenter _mockPresenter2;

    private CompositePresenter _compositePresenter;

    [SetUp]
    public void Setup()
    {
        _mockPresenter1 = new MockPresenter();
        _mockPresenter2 = new MockPresenter();

        _compositePresenter = new CompositePresenter(new IPresenter[] { _mockPresenter1, _mockPresenter2 });
        _compositePresenter.Activate();

    }

    [Test]
    public void CompositePresenter_Activate_登録された複数のPresenterをActivateする()
    {

        Assert.That(_mockPresenter1.IsActivated && _mockPresenter2.IsActivated, Is.True);
    }

    [Test]
    public void CompositePresenter_Deactivate_登録された複数のPresenterをDeactivateする()
    {
        _compositePresenter.Deactivate();
        Assert.That(_mockPresenter1.IsDeactivated && _mockPresenter2.IsDeactivated, Is.True);
    }
}