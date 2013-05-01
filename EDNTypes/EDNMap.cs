//   Copyright (c) Thortech Solutions, LLC. All rights reserved.
//   The use and distribution terms for this software are covered by the
//   Eclipse Public License 1.0 (http://opensource.org/licenses/eclipse-1.0.php)
//   which can be found in the file epl-v10.html at the root of this distribution.
//   By using this software in any fashion, you are agreeing to be bound by
//   the terms of this license.
//   You must not remove this notice, or any other, from this software.
//
//   Authors: Dimitrios Kapsalis, Mark Perrotta
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public class EDNMap : IEnumerable<KeyValuePair<object, object>>, IEDNEnumerable, IEDNWritable, ICollection, IDictionary, IDictionary<object, object>
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
            return ((IEnumerable<KeyValuePair<object, object>>)this).GetEnumerator();
        }

        #endregion

        #region IEDNEnumerable Members

        public int Count()
        {
            return ednMap.Count + nullKeyValuePair.Count();
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


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (ReferenceEquals(this, obj))
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
                    if (!this.ContainsKey(kvp.Key))
                        return false;
                    else if (ReferenceEquals(this[kvp.Key], kvp.Value)) //check that values have same reference (including null)
                        continue;
                    else if (this[kvp.Key] != null && !this[kvp.Key].Equals(kvp.Value))
                        return false;
                    else if (this[kvp.Key] == null && kvp.Value != null)
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


        #region IEDNPrintable Members

        public string WriteEDN(IWriteHandler handler)
        {
            return Utils.WritePrintableToString(this, handler);
        }

        public void WriteEDN(System.IO.Stream stream, IWriteHandler handler)
        {
            stream.Write(Utils.openMapBytes, 0, Utils.openMapBytes.Length);
            handler.handleEnumerable(this, stream,this);
            stream.Write(Utils.closeMapBytes, 0, Utils.closeMapBytes.Length);
        }

        #endregion

        #region IDictionary Members

        public void Add(object key, object value)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object key)
        {
            return ContainsKey(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return  new EDNDictEnumerator(this);
        }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        ICollection IDictionary.Keys
        {
            get { throw new NotSupportedException(); }
        }

        public void Remove(object key)
        {
            throw new NotSupportedException();
        }

        ICollection IDictionary.Values
        {
            get { throw new NotSupportedException(); }
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
            set
            {
                throw new NotSupportedException();
            }
        }
       
        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            int n = index;
            foreach (var kvp in this)
            {
                array.SetValue(kvp, n);
                n++;
            }
        }

        int ICollection.Count
        {
            get { return Count(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotSupportedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotSupportedException(); }
        }

        #endregion

        #region IDictionary<object,object> Members


        public ICollection<object> Keys
        {
            get {
                var keys = ednMap.Keys.ToList();

                if (nullKeyValuePair.Count > 0)
                    return keys.Concat(new object[] { nullKeyValuePair.First().Key }).ToList();
                else
                    return keys;
            }
        }

        bool IDictionary<object, object>.Remove(object key)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(object key, out object value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                var values = ednMap.Values.ToList();

                if (nullKeyValuePair.Count > 0)
                    return values.Concat(new object[] { nullKeyValuePair.First().Value }).ToList();
                else
                    return values;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<object,object>> Members

        public void Add(KeyValuePair<object, object> item)
        {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<object, object> item)
        {
            foreach (var kvp in this)
            {
                if (kvp.Key.Equals(item.Key) && kvp.Value.Equals(item.Value))
                    return true;
            }

            return false;
        }

        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            this.CopyTo((Array)array, arrayIndex);
        }

        int ICollection<KeyValuePair<object, object>>.Count
        {
            get { return this.Count(); }
        }

        public bool Remove(KeyValuePair<object, object> item)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    public class EDNDictEnumerator : IDictionaryEnumerator
    {
        IEnumerator _enumerator;

        public EDNDictEnumerator(IEnumerable enumerable)
        {
            _enumerator = enumerable.GetEnumerator();
        }

        public DictionaryEntry Entry
        {
            get { return new DictionaryEntry(((KeyValuePair<object, object>)_enumerator.Current).Key, 
                    ((KeyValuePair<object, object>)_enumerator.Current).Value); }
        }

        public object Key
        {
            get { return ((KeyValuePair<object, object>)_enumerator.Current).Key; }
        }

        public object Value
        {
            get { return ((KeyValuePair<object, object>)_enumerator.Current).Value; }
        }

        #region IEnumerator Members

        public object Current
        {
            get { return _enumerator.Current; }
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        #endregion
    }
}
