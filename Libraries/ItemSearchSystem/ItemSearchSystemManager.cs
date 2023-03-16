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

        public ItemSearchSystemManager(Searcher searcher, Taker taker, Register register)
        {
            _searcher = searcher;
            _taker = taker;
            _register = register;
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