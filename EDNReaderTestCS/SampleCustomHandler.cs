using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EDNTypes;

using EDNReaderWriter;

namespace EDNReaderTestCS
{
    /// <summary>
    /// Sample EDN reader handler. Converts EDN maps to Dictionary<object, object> instead of EDNMap.
    /// </summary>
    public class SampleCustomHandler : TypeHandlers.BaseTypeHandler
    {
        public override object handleValue(EDNParserTypes.EDNValueParsed ednValueParsed)
        {
            if (ednValueParsed.ednValue.IsEDNMap)
            {
                Dictionary<object, object> dict = new Dictionary<object, object>();

                var enumerator = ((EDNParserTypes.EDNValue.EDNMap)ednValueParsed.ednValue).Item.AsEnumerable().GetEnumerator();

                while (enumerator.MoveNext())
                {
                    var key = handleValue(enumerator.Current);
                    if (!enumerator.MoveNext())
                        throw new Exception("Map must have even number of elements");

                    var value = handleValue(enumerator.Current);
                    dict.Add(key, value);
                }

                return dict;
            }
            else
                return base.handleValue(ednValueParsed);
        }
    }
}
