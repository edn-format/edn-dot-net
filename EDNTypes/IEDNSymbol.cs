using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public interface IEDNSymbol : IComparable
    {
        string getPrefix();

        string getName();
    }
}
