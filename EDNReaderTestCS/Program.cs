using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using EDNTypes;

using EDNReader;


namespace EDNReaderTestCS
{

    class Program
    {
        static void Main(string[] args)
        {
            //var r1 = EDNReader.EDNReader.parseString("[\n:pre/asdf [1 2 3 \"Asdfsaf\" 5 6 7 #{\"a\" 1 2 [7 8 9]} {[1 2] #{78 12} \"asdfa\" 4} ]]");
            var r1 = EDNReader.EDNReader.parseFile("C:\\dev\\edn-test-data\\hierarchical.edn");

            foreach (var ednObj in r1)
            {
                string teststr = ((IEDNPrintable)ednObj).PrintEDN();
                using (var ms = new MemoryStream())
                {

                    PrintUtils.PrintEDNObjectToStream(ednObj, ms);

                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var myStr = sr.ReadToEnd();
                }
            }
            

            //File parsting test
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var rDir = EDNReader.EDNReader.parseDirectory("C:\\dev\\edn-test-data\\hierarchy_noisy_500");
            sw.Stop();

            Console.WriteLine(String.Format("Elapsed MS: {0}", sw.ElapsedMilliseconds));

            var rFile = EDNReader.EDNReader.parseFile("C:\\dev\\edn-test-data\\noisy.edn");

            /*
            var r2 = Funcs.Flatten(r1);

            foreach (var e in r2)
            {
                Console.WriteLine(e);
            }
             */


        }
    }
}
