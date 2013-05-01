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

using System.IO;

namespace EDNTypes
{
    public interface IEDNWritable
    {
        /// <summary>
        /// Serializes the object graph to an EDN string. WARNING: should not be used for large graphs as single large strings
        /// (larger than 80kb) end up on the Large Object Heap(LOH). If the LOH fills up frequently, the garbage collector will
        /// be forced to collect "the world" more often. Consider using the PrintEDN(stream) method.
        /// </summary>
        /// <returns>The EDN string</returns>
        string WriteEDN(IWriteHandler handler);

        /// <summary>
        /// Serializes the object graph as an EDN string to the stream.
        /// </summary>
        /// <param name="stream"></param>
        void WriteEDN(Stream stream, IWriteHandler handler);
    }
}
