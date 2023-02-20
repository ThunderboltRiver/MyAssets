using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InputableActor
{
    internal struct Event<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; }

        internal Event(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}