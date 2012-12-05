using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public class EDNSymbol : IEDNSymbol, IEDNPrintable
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

        public string PrintEDN(IPrintHandler handler)
        {
            return Utils.getSymbolString(prefix, name);
        }

        public void PrintEDN(System.IO.Stream stream, IPrintHandler handler)
        {
            PrintUtils.WriteEDNToStream(this.ToString(), stream);
        }

        #endregion
    }
}
