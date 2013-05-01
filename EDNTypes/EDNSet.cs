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
    public class EDNSet : IEnumerable<object>, IEDNEnumerable, IEDNWritable
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
            if (obj == null)
                return false;
            else if (ReferenceEquals(this, obj))
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

        public string WriteEDN(IWriteHandler handler)
        {
            return Utils.WritePrintableToString(this, handler);
        }

        public void WriteEDN(System.IO.Stream stream, IWriteHandler handler)
        {
            stream.Write(Utils.openSetBytes, 0, Utils.openSetBytes.Length);
            handler.handleEnumerable(this, stream,this);
            stream.Write(Utils.closeSetBytes, 0, Utils.closeSetBytes.Length);
        }

        #endregion
    }
}
