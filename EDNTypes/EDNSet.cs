using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public class EDNSet : IEnumerable<object>, IEDNEnumerable, IEDNPrintable
    {
        private HashSet<object> ednHashSet = null;

        public EDNSet(IEnumerable coll)
        {
            ednHashSet = new HashSet<object>();

            foreach (var e in coll)
            {
                if (ednHashSet.Contains(e))
                    throw new Exception(String.Format("Duplicate set entry: {0}", e == null ? "NULL" : e.ToString()));
                else
                    ednHashSet.Add(e);
            }
                
        }

        #region IEDNEnumerable Members

        public int Count()
        {
            return ednHashSet.Count;
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return ednHashSet.GetEnumerator();
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return ednHashSet.GetEnumerator();
        }

        #endregion


        public bool Contains(object obj)
        {
            return ednHashSet.Contains(obj);
        }

        public override int GetHashCode()
        {
            return Utils.additionHashCode(ednHashSet);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            else if (obj.GetType() != typeof(EDNSet))
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
                    if (!ednHashSet.Contains(enum1.Current))
                        return false;

                }

                return true;
            }
        }

        public static bool operator ==(EDNSet obj1, object obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EDNSet obj1, object obj2)
        {
            return !obj1.Equals(obj2);
        }

        #region IEDNPrintable Members

        static readonly Byte[] openSetBytes = Encoding.UTF8.GetBytes("#{");
        static readonly Byte[] closeSetBytes = Encoding.UTF8.GetBytes("}");

        public string PrintEDN()
        {
            return PrintUtils.WriteStreamToString(this);
        }

        public void PrintEDN(System.IO.Stream stream)
        {
            stream.Write(openSetBytes, 0, openSetBytes.Length);
            PrintUtils.PrintIEnumerableToEDN(this, stream);
            stream.Write(closeSetBytes, 0, closeSetBytes.Length);
        }

        #endregion
    }
}
