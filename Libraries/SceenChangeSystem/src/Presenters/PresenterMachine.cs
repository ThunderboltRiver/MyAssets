using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;

namespace SceenChangeSystem.Presenters
{
    /// <summary>
    /// Presenterを切り替えるクラス
    /// </summary>
    /// <typeparam name="T">Presenterに紐付けるキーの型</typeparam>
    public class PresenterMachine<T>
    {
        /// <summary>
        /// Presenterを管理する連想配列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="IPresenter"></typeparam>
        /// <returns></returns>
        private readonly Dictionary<T, IPresenter> _presenters = new Dictionary<T, IPresenter>();

        /// <summary>
        /// Presenterを切り替える前に呼ばれるSubject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private readonly Subject<T> _beforePresenterChange = new Subject<T>();

        /// <summary>
        /// Presenterを切り替えるタイミングを監視するDisposable
        /// </summary>
        private IDisposable _disposable;

        /// <summary>
        /// Presenterを切り替えるタイミングを発行するObservable
        /// </summary>
        private IObservable<T> _eventObservable;

        /// <summary>
        /// PresenterMachineのコンストラクタ.
        /// </summary>
        /// <param name="eventObservable">Presenterを切り替えるタイミングを発行するObsrvable</param>
        public PresenterMachine(IObservable<T> eventObservable)
        {
            _eventObservable = eventObservable;
        }

        /// <summary>
        /// PresenterをPresenterMachineに紐付ける
        /// </summary>
        /// <param name="presenterKey">Presenterにアクセスするためのキー</param>
        /// <param name="presenter">紐付けたいPresenter</param>
        public void BindPresenter(T presenterKey, IPresenter presenter)
        {
            _presenters.Add(presenterKey, presenter);
        }

        /// <summary>
        /// Presenterを切り替える前のイベントを購読可能にするObservable
        /// </summary>
        public IObservable<T> BeforePresenterChangeAsObservable => _beforePresenterChange;

        /// <summary>
        /// 現在のPresenter
        /// </summary>
        private IPresenter _currentPresenter;

        /// <summary>
        /// 現在のPresenterを切り替える
        /// </summary>
        /// <param name="presenterKey">切り替えたいPresenterのキー</param>
        /// <returns></returns>
        public void ChangeCurrentPresenter(T presenterKey)
        {
            _currentPresenter?.Deactivate();
            _currentPresenter = _presenters[presenterKey];
            _beforePresenterChange.OnNext(presenterKey);
            _currentPresenter.Activate();
        }

        /// <summary>
        /// このPresenterMachineを有効化する
        /// </summary>
        public void Activate()
        {
            _disposable = _eventObservable.Subscribe(presenterKey => ChangeCurrentPresenter(presenterKey));
        }

        /// <summary>
        /// このPresenterMachineを無効化する
        /// </summary>
        public void Deactivate()
        {
            _disposable.Dispose();
            _currentPresenter?.Deactivate();
        }
    }
}