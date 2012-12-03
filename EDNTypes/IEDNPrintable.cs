using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public interface IEDNPrintable
    {
        /// <summary>
        /// Serializes the object graph to an EDN string. WARNING: should not be used for large graphs as single large strings
        /// (larger than 80kb) end up on the Large Object Heap(LOH). If the LOH fills up frequently, the garbage collector will
        /// be forced to collect "the world" more often. Consider using the PrintEDN(stream) method.
        /// </summary>
        /// <returns>The EDN string</returns>
        string PrintEDN();

        /// <summary>
        /// Serializes the object graph as an EDN string to the stream.
        /// </summary>
        /// <param name="stream"></param>
        void PrintEDN(Stream stream);
    }
}
