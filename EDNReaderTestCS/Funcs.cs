using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNReaderTestCS
{
    public static class Funcs
    {
        public static IEnumerable Flatten(IEnumerable enumerable)
        {
            foreach (object element in enumerable)
            {
                IEnumerable candidate = element is string ? null : element as IEnumerable;

                if (candidate != null)
                {
                    foreach (object nested in Flatten(candidate))
                    {
                        yield return nested;
                    }
                }
                else if (element is KeyValuePair<object, object>)
                {
                    var kvp = (KeyValuePair<object, object>)element;

                    IEnumerable key = kvp.Key is string ? null : kvp.Key as IEnumerable;
                    IEnumerable value = kvp.Value is string ? null : kvp.Value as IEnumerable;
                    if (key != null)
                    {
                        foreach (object nestedKey in Flatten(key))
                        {
                            yield return nestedKey;
                        }
                    }
                    else
                        yield return kvp.Key;

                    if (value != null)
                    {
                        foreach (object nestedValue in Flatten(value))
                        {
                            yield return nestedValue;
                        }
                    }
                    else
                        yield return kvp.Value;

                }
                else
                {
                    yield return element;
                }
            }
        }


        public static void Assert(bool assertion)
        {
            if(!assertion)
                throw new Exception("Assert failed.");
        }

    }
}
