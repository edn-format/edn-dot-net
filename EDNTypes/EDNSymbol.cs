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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public class EDNSymbol : IEDNSymbol, IEDNWritable
    {
        private string prefix;
        private string name;

        public EDNSymbol(string prefix, string name)
        {
            this.prefix = prefix;
            this.name = name;
        }

        public override string ToString()
        {
            return Utils.getSymbolString(prefix, name);
        }

        public override int GetHashCode()
        {
            return Utils.getSymbolString(prefix, name).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if(System.Object.ReferenceEquals(this, obj))
                return true;
            else if(obj.GetType() != typeof(EDNSymbol))
                return false;
            else if(this.ToString() == obj.ToString())
                return true;
            else
                return false;
        }

        #region IEDNSymbol Members

        public string getPrefix()
        {
            return prefix;
        }

        public string getName()
        {
            return name;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return Utils.getSymbolString(prefix, name).CompareTo(obj.ToString());
        }

        #endregion

        #region IEDNPrintable Members

        public string WriteEDN(IWriteHandler handler)
        {
            return Utils.getSymbolString(prefix, name);
        }

        public void WriteEDN(System.IO.Stream stream, IWriteHandler handler)
        {
            Utils.WriteEDNToStream(this.ToString(), stream);
        }

        #endregion
    }
}
