using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public static class Utils
    {
        public static int additionHashCode(IEnumerable coll)
        {
            int hash = 0;
            foreach (var obj in coll)
            {
                hash = hash + (obj == null ? 0 : obj.GetHashCode());
            }
            return hash;
        }
    }
}
