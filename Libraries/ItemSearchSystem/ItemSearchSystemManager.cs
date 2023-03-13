using UnityEngine;
using ObservableCollections;
using System.Collections.Specialized;
using System;
using UniRx;
using System.Collections.Concurrent;

namespace ItemSearchSystem
{
    public class ItemSearchSystemManager
    {
        private Searcher _searcher;
        private Taker _taker;
        private Register _register;
        public IObservableCollection<ITakable> WaitingTakablesAsObservableCollection => _taker.WaitingTakablesAsObservableCollection;

        private Subject<NotifyCollectionChangedAction> _watingTakablesAddAsObservable = new();
        public IObservable<NotifyCollectionChangedAction> WaitingTakablesAddAsObservable => _watingTakablesAddAsObservable;

        public ItemSearchSystemManager(Searcher searcher, Taker taker, Register register)
        {
            _searcher = searcher;
            _taker = taker;
            _register = register;
            _taker.WaitingTakablesAsObservableCollection.CollectionChanged += (in NotifyCollectionChangedEventArgs<ITakable> args) =>
            {
                if (args.Action == NotifyCollectionChangedAction.Add)
                {
                    _watingTakablesAddAsObservable.OnNext(args.Action);
                }
            };
        }

        public bool SearchAndTryPushTakable()
        {
            if (_searcher.Search(out GameObject gameObject))
            {
                return _taker.TryPushTakable(gameObject);
            }
            _taker.ClearTakable();
            return false;
        }
        public bool TakeAndTryRegist()
        {
            return _taker.Take(out object obj) && _register.TryRegist(obj);
        }


    }

}