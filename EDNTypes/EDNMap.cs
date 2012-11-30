using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public class EDNMap : IEnumerable<KeyValuePair<object, object>>, IEDNEnumerable
    {
        private Dictionary<object, object> ednMap;

        private readonly List<KeyValuePair<object, object>> nullKeyValuePair = new List<KeyValuePair<object,object>>();

        public EDNMap(IDictionary<object, object> dictionary)
        {
            ednMap = new Dictionary<object, object>();
            foreach (var kvp in dictionary)
            {
                if (kvp.Key == null)
                    nullKeyValuePair.Add(kvp);
                else
                    ednMap.Add(kvp.Key, kvp.Value);
            }
        }


        public EDNMap(IEnumerable<object> coll)
        {
            ednMap = new Dictionary<object, object>();

            if(coll.Count() % 2 != 0)
                throw new Exception("EDNMap initialization collection must have an even number of elements.");

            var enumerator = coll.GetEnumerator();
            while (enumerator.MoveNext())
            {
                object key = enumerator.Current;
                enumerator.MoveNext();
                object value = enumerator.Current;

                if (key == null)
                    nullKeyValuePair.Add(new KeyValuePair<object, object>(key, value));
                else
                    ednMap.Add(key, value);
            }
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            var dictionary = ednMap.Concat(nullKeyValuePair);
            return dictionary.GetEnumerator();
        }

        #endregion

        #region IEDNEnumerable Members

        public int Count()
        {
            return ednMap.Count;
        }

        #endregion

        #region IEnumerable<KeyValuePair<object,object>> Members

        IEnumerator<KeyValuePair<object, object>> IEnumerable<KeyValuePair<object, object>>.GetEnumerator()
        {
            var dictionary = nullKeyValuePair.Concat(ednMap);
            return dictionary.GetEnumerator();
        }

        #endregion

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var keyValue in ednMap)
            {
                hash += (keyValue.Key == null ? 0 : keyValue.Key.GetHashCode()) ^
                        (keyValue.Value == null ? 0 : keyValue.Value.GetHashCode());
            }
            return hash;
        }

        public bool ContainsKey(object key)
        {
            if (key == null && nullKeyValuePair.Count == 0)
                return false;
            else if (key == null && nullKeyValuePair.Count > 0)
                return true;
            else
                return ednMap.ContainsKey(key);
        }

        public object this[object key]
        {
            get
            {
                if (key == null && nullKeyValuePair.Count == 0)
                    throw new KeyNotFoundException("Could not find key: NULL");
                else if (key == null && nullKeyValuePair.Count > 0)
                    return nullKeyValuePair.First().Value;
                else
                    return ednMap[key];
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            else if (obj.GetType() != typeof(EDNMap))
                return false;
            else if (obj.GetHashCode() != this.GetHashCode())
                return false;
            else
            {
                IEDNEnumerable e = (IEDNEnumerable)obj;

                if (e.Count() != this.Count())
                    return false;

                var enum1 = e.GetEnumerator();
                while (enum1.MoveNext())
                {
                    var kvp = (KeyValuePair<object, object>)enum1.Current;
                    if (!ednMap.ContainsKey(kvp.Key))
                        return false;
                    else if (!ednMap[kvp.Key].Equals(kvp.Value))
                        return false;
                }

                return true;
            }
        }

        public static bool operator ==(EDNMap obj1, object obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EDNMap obj1, object obj2)
        {
            return !obj1.Equals(obj2);
        }

    }
}
