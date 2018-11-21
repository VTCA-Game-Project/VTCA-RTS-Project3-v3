using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Utils
{
    public class QueryItem<Key, Value>
    {
        public Key key;
        public Value value;

        public QueryItem(Key argKey, Value argValue)
        {
            key = argKey;
            value = argValue;
        }
    }

    public class QueryList<Key, Value>
    {
        private List<QueryItem<Key, Value>> dict;
        public int Count
        {
            get { return dict.Count; }
        }

        public QueryList()
        {
            dict = new List<QueryItem<Key, Value>>();
        }

        public void Add(QueryItem<Key, Value> queryItem, bool overrideKey)
        {
            if (overrideKey)
            {
                Remove(queryItem.key);
            }
            dict.Add(queryItem);
        }

        public bool TryGetValue(Key key, out Value value)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict[i].key.Equals(key))
                {
                    value = dict[i].value;
                    return true;
                }
            }
            value = default(Value);
            return false;
        }

        public bool TryGetQueryItem(Key key, out QueryItem<Key, Value> item)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict[i].key.Equals(key))
                {
                    item = dict[i];
                    return true;
                }
            }

            item = default(QueryItem<Key, Value>);
            return false;
        }

        public bool ContainsKey(Key key)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict[i].key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Remove(Key key)
        {
            QueryItem<Key, Value> queryItem;
            if (TryGetQueryItem(key, out queryItem))
            {
                dict.RemoveAt(dict.IndexOf(queryItem));
                return true;
            }
            return false;
        }

        public List<QueryItem<Key, Value>> QueryItemList()
        {
            return dict;
        }

        public int IndexOf(QueryItem<Key, Value> item)
        {
            return dict.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            if (index >= 0)
                dict.RemoveAt(index);
        }

        public void Clear()
        {
            dict.Clear();
        }
    }

}