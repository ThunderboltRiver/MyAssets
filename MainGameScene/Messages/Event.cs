using System;
namespace MainGameScene.Messages
{
    public interface IMessage
    {

    }

    public struct Event<TValue, TKey>
    {
        private TValue _value;
        private TKey _key;

        public TValue Value() { return _value; }
        public TKey Key() { return _key; }
        public String Context() { return ""; }

        public Event(TValue value, TKey key)
        {
            _value = value;
            _key = key;
        }
    }

    public struct OutEvent<TValue, TContext>
    {
        private TValue _value;
        private TContext _context;
        public TValue Value() => _value;
        public int Key() => -1;

        public TContext Context() => _context;

        public OutEvent(TValue value, TContext context)
        {
            _value = value;
            _context = context;
        }

    }
}