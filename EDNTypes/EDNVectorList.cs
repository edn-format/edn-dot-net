using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public class EDNVectorList : IEnumerable<object>, IEDNEnumerable
    {
        private List<object> ednList;

        public EDNVectorList(IEnumerable<object> coll)
        {
            ednList = new List<object>(coll);
        }

        public int IndexOf(object item)
        {
            return ednList.IndexOf(item);
        }

        public object this[int index]
        {
            get
            {
                return ednList[index];
            }
        }

        public bool Contains(object item)
        {
            return ednList.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            ednList.CopyTo(array, arrayIndex);
        }

        public int Count()
        {
            return ednList.Count; 
        }

        #region IEnumerable<object> Members

        public IEnumerator<object> GetEnumerator()
        {
            return ednList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ednList.GetEnumerator();
        }

        #endregion

        public override int GetHashCode()
        {
            return Utils.additionHashCode(ednList);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            else if (obj.GetType() != typeof(EDNVectorList))
                return false;
            else if (obj.GetHashCode() != this.GetHashCode())
                return false;
            else
            {
                IEDNEnumerable e = (IEDNEnumerable)obj;

                if (e.Count() != this.Count())
                    return false;

                var enum1 = e.GetEnumerator();
                var enum2 = this.GetEnumerator();

                while (enum1.MoveNext() && enum2.MoveNext())
                {
                    if (!enum1.Current.Equals(enum2.Current))
                        return false;
                }

                return true;
            }
        }

        public static bool operator ==(EDNVectorList obj1, object obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EDNVectorList obj1, object obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}
