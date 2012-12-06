using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDNTypes;
using EDNReaderWriter;

namespace EDNReaderTestCS
{
    public class SampleCustomPrinter : PrintHandlers.BasePrintHandler
    {
        private static readonly Byte[] tagBytes = Encoding.UTF8.GetBytes("#timezone ");
        private static readonly Byte[] keyDaylightNameBytes = Encoding.UTF8.GetBytes(":daylight-name");
        private static readonly Byte[] keyStandardNameBytes = Encoding.UTF8.GetBytes(":standard-name");

        public override void handleObject(object obj, System.IO.Stream stream)
        {
            // If obj is a type that I want to handle, in this case System.TimeZone, handle it here.
            // Otherwise run the default BasePrinterHandler
            if (obj.GetType() == typeof(System.TimeZone))
            {
                System.TimeZone tz = (System.TimeZone)obj;

                stream.Write(tagBytes, 0, tagBytes.Length);
                stream.Write(Utils.openMapBytes, 0, Utils.openMapBytes.Length);
                
                stream.Write(keyDaylightNameBytes, 0, keyDaylightNameBytes.Length);
                Utils.WriteEDNToStream(tz.DaylightName, stream);

                stream.Write(keyStandardNameBytes, 0, keyStandardNameBytes.Length);
                Utils.WriteEDNToStream(tz.StandardName, stream);

                stream.Write(Utils.closeMapBytes, 0, Utils.closeMapBytes.Length);
            }
            else
                base.handleObject(obj, stream);
        }
    }
}
