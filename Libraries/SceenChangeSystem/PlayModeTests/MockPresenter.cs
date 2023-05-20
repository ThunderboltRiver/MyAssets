using SceenChangeSystem.Presenters;



internal class MockPresenter : IPresenter
{
    bool _isActivated = false;
    public bool IsActivated => _isActivated;
    bool _isDeactivated = false;
    public bool IsDeactivated => _isDeactivated;
    public void Activate()
    {
        _isActivated = true;
    }

    public void Deactivate()
    {
        _isDeactivated = true;
    }
}
