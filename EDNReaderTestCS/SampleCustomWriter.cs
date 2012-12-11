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
using EDNTypes;
using EDNReaderWriter;

namespace EDNReaderTestCS
{
    /// <summary>
    /// Sample Writer/writer handler. Has the following custom behavior:
    /// - Handles System.TimeZoneInfo via a special tag. See the SampleCustomHandler for reading this type. 
    /// </summary>
    public class SampleCustomWriter : WriteHandlers.BaseWriteHandler
    {
        private static readonly Byte[] tagBytes = Encoding.UTF8.GetBytes("#sample-custom-type/timezone ");
        private static readonly Byte[] keyIdBytes = Encoding.UTF8.GetBytes(":Id");
        private static readonly Byte[] keyDisplayNameBytes = Encoding.UTF8.GetBytes(":DisplayName");
        private static readonly Byte[] keyBaseUtcOffsetBytes = Encoding.UTF8.GetBytes(":BaseUtcOffset");
        private static readonly Byte[] keyStandardDisplayNameBytes = Encoding.UTF8.GetBytes(":StandardDisplayName");

        public override void handleObject(object obj, System.IO.Stream stream)
        {
            // If obj is a type that I want to handle, in this case System.TimeZoneInfo, handle it here.
            // Otherwise run the default BaseWriterHandler
            if (obj.GetType() == typeof(System.TimeZoneInfo))
            {
                System.TimeZoneInfo tz = (System.TimeZoneInfo)obj;

                stream.Write(tagBytes, 0, tagBytes.Length);
                stream.Write(Utils.openMapBytes, 0, Utils.openMapBytes.Length);

                stream.Write(keyIdBytes, 0, keyIdBytes.Length);
                Utils.WriteEDNToStream(string.Format(" \"{0}\" ", tz.Id), stream);

                stream.Write(keyDisplayNameBytes, 0, keyDisplayNameBytes.Length);
                Utils.WriteEDNToStream(string.Format(" \"{0}\" ", tz.DisplayName), stream);

                stream.Write(keyBaseUtcOffsetBytes, 0, keyBaseUtcOffsetBytes.Length);
                Utils.WriteEDNToStream(string.Format(" \"{0}\" ", tz.BaseUtcOffset.ToString()), stream);

                stream.Write(keyStandardDisplayNameBytes, 0, keyStandardDisplayNameBytes.Length);
                Utils.WriteEDNToStream(string.Format(" \"{0}\" ", tz.StandardName), stream);

                stream.Write(Utils.closeMapBytes, 0, Utils.closeMapBytes.Length);
            }
            else
                base.handleObject(obj, stream);
        }

        public override void handleEnumerable(System.Collections.IEnumerable enumerable, System.IO.Stream stream)
        {
            base.handleEnumerable(enumerable, stream);
        }
    }
}
