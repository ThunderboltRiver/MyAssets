using System;
using System.Collections.Generic;
using UnityEngine;
namespace ItemSearchSystem
{
    public class ItemSearchSystemManager
    {
        private Searcher _searcher;
        private Taker _taker;
        private Register _register;
        public ItemSearchSystemManager(Searcher searcher, Taker taker, Register register)
        {
            _searcher = searcher;
            _taker = taker;
            _register = register;
        }

        public bool SearchAndTryPushTakable()
        {
            return _searcher.Search(out GameObject gameObject) && _taker.TryPushTakable(gameObject);
        }

        public bool TakeAndTryRegist()
        {
            return _taker.Take(out object obj) && _register.TryRegist(obj);
        }
    }
}