using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;

namespace SceenChangeSystem.Presenters
{
    public class RenderingPresenter<T> : IPresenter
    {
        private readonly IDisposableCreator<T> _viewCreator;
        private readonly IObservable<T> _eventObservable;

        private IDisposable _disposable;
        private IDisposable _createdViewDisposable;

        public RenderingPresenter(IDisposableCreator<T> viewCreator, IObservable<T> eventObservable)
        {
            _viewCreator = viewCreator;
            _eventObservable = eventObservable;
        }
        public void Activate()
        {
            _disposable = _eventObservable.Subscribe(data =>
            {
                _createdViewDisposable?.Dispose();
                _createdViewDisposable = _viewCreator.Create(data);
            });
        }

        public void Deactivate()
        {
            _disposable?.Dispose();
            _createdViewDisposable?.Dispose();
        }
    }
}