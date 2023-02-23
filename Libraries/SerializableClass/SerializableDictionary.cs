using System;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableClass
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {

        [SerializeField]
        private List<KeyAndValue<TKey, TValue>> list;
        private Dictionary<TKey, TValue> dict;

        public Dictionary<TKey, TValue>.KeyCollection Keys => GetDict().Keys;
        public Dictionary<TKey, TValue>.ValueCollection Values => GetDict().Values;



        public Dictionary<TKey, TValue> GetDict()
        {
            if (dict == null)
            {
                dict = ConvertListToDictionary(list);
            }
            return dict;
        }

        /// <summary>
        /// Editor Only
        /// </summary>
        public List<KeyAndValue<TKey, TValue>> GetList()
        {
            return list;
        }

        static Dictionary<TKey, TValue> ConvertListToDictionary(List<KeyAndValue<TKey, TValue>> list)
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
            foreach (KeyAndValue<TKey, TValue> pair in list)
            {
                dic.Add(pair.Key, pair.Value);
            }
            return dic;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return GetDict().TryGetValue(key, out value);
        }
        public bool TryAdd(TKey key, TValue value)
        {

            if (GetDict().TryAdd(key, value))
            {
                list.Add(new KeyAndValue<TKey, TValue>(key, value));
                return true;
            };
            return false;
        }

        public void Clear()
        {
            GetDict().Clear();
            list.Clear();
        }

    }

    /// <summary>
    /// シリアル化できる、KeyValuePair
    /// </summary>
    [Serializable]
    public class KeyAndValue<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public KeyAndValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public KeyAndValue(KeyValuePair<TKey, TValue> pair)
        {
            Key = pair.Key;
            Value = pair.Value;
        }


    }

}
