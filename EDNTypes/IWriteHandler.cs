using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace EDNTypes
{
    public interface IWriteHandler
    {
        void handleObject(object obj, Stream stream);

        void handleEnumerable(IEnumerable enumerable, Stream stream);
    }
}
