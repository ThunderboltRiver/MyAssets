using System.Linq;

namespace SceenChangeSystem.Presenters
{
    public class CompositePresenter : IPresenter
    {
        private readonly IPresenter[] _presenters;

        public CompositePresenter(IPresenter[] presenters)
        {
            _presenters = presenters;
        }
        public void Activate()
        {
            foreach (IPresenter presenter in _presenters)
            {
                presenter.Activate();
            }
        }

        public void Deactivate()
        {
            foreach (IPresenter presenter in _presenters)
            {
                presenter.Deactivate();
            }
        }
    }
}