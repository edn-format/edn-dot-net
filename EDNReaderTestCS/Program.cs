using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using EDNTypes;

using EDNReaderWriter;


namespace EDNReaderTestCS
{

    class Program
    {
        static void Main(string[] args)
        {
            
            TestSampleCustomHandler();
            TestParseString();
            TestParseFile();
            TestWriter();
            TestParseDirectory();

        }

        public static void TestSampleCustomHandler()
        {
            var r1 = EDNReader.EDNReaderFuncs.parseString("[1 2 {:a 3 1.2 \\c {1 2} 4}]", new SampleCustomHandler());
        }

        public static void TestParseString()
        {
            var r1 = EDNReader.EDNReaderFuncs.parseString("[\n:pre/asdf [1 2 3 \"Asdfsaf\" 5 6 7 #{\"a\" 1 2 [7 8 9]} {[1 2] #{78 12} \"asdfa\" 4} ]]");
        }

        public static void TestParseFile()
        {
            var r1 = EDNReader.EDNReaderFuncs.parseFile("C:\\dev\\edn-test-data\\hierarchical.edn");
        }

        public static void TestWriter()
        {
            var r1 = EDNReader.EDNReaderFuncs.parseString("[\n:pre/asdf [1 2 3 \"Asdfsaf\" 5 6 7 #{\"a\" 1 2 [7 8 9]} {[1 2] #{78 12} \"asdfa\" 4} ]]");

            foreach (var ednObj in r1)
            {
                string teststr = ((IEDNPrintable)ednObj).PrintEDN();
                Console.WriteLine("Print String: {0}", teststr);
                using (var ms = new MemoryStream())
                {

                    PrintUtils.PrintEDNObjectToStream(ednObj, ms);

                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var myStr = sr.ReadToEnd();
                    Console.WriteLine("Print Stream: {0}", myStr);
                }
            }
            
        }

        public static void TestParseDirectory()
        {
            //File parsting test
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var rDir = EDNReader.EDNReaderFuncs.parseDirectory("C:\\dev\\edn-test-data\\hierarchy_noisy_500");
            sw.Stop();

            Console.WriteLine(String.Format("Elapsed MS: {0}", sw.ElapsedMilliseconds));
        }
    }
}
