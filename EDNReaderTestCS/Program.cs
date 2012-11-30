using System;
using System.Collections;
using System.Collections.Generic;
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
            var r1 = EDNReader.EDNReader.parseString("[\n[1 2 3 {:a 5} \"Asdfsaf\" 5 6 7]]");

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
