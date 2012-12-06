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

            TestEquality();
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
            var r1 = EDNReader.EDNReaderFuncs.parseString(TEST_STR_SmallHierarchy);
        }

        public static void TestEquality()
        {
            var r1 = EDNReader.EDNReaderFuncs.parseString(TEST_STR_SmallHierarchy).First();
            var r2 = EDNReader.EDNReaderFuncs.parseString(TEST_STR_SmallHierarchy).First();
            Funcs.Assert(r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.parseString("[[nil 1 nil]]").First();
            r2 = EDNReader.EDNReaderFuncs.parseString("[[1 nil 1]]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.parseString("[[nil 1 nil]]").First();
            r2 = EDNReader.EDNReaderFuncs.parseString("[[1 nil]]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.parseString("[{nil 1 1 nil}]").First();
            r2 = EDNReader.EDNReaderFuncs.parseString("[{nil 1 1 2}]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.parseString("[(nil 1 1 2)]").First();
            r2 = EDNReader.EDNReaderFuncs.parseString("[(4 1 1 nil)]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.parseString("[#{nil 1 2 3}]").First();
            r2 = EDNReader.EDNReaderFuncs.parseString("[#{nil 1 2 3}]").First();
            Funcs.Assert(r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.parseString("[#{nil 1 2 3}]").First();
            r2 = EDNReader.EDNReaderFuncs.parseString("[#{5 nil 1 6}]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.parseFile("..\\..\\..\\TestData\\hierarchical.edn");

            r2 = EDNReader.EDNReaderFuncs.parseFile("..\\..\\..\\TestData\\hierarchical.edn");

            Funcs.Assert(r1.Equals(r2));
        }

        public static void TestParseFile()
        {
            var r1 = EDNReader.EDNReaderFuncs.parseFile("..\\..\\..\\TestData\\hierarchical.edn");
        }

        public static void TestWriter()
        {
            var r1 = EDNReader.EDNReaderFuncs.parseString(TEST_STR_SmallHierarchy);

            foreach (var ednObj in r1)
            {
                string teststr = EDNWriter.EDNWriterFuncs.writeString(ednObj);
                Console.WriteLine("Print String: {0}", teststr);
                using (var ms = new MemoryStream())
                {

                    EDNWriter.EDNWriterFuncs.writeStream(ednObj, ms);

                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var stringFromStream = sr.ReadToEnd();
                    Funcs.Assert(teststr.Equals(stringFromStream));
                    Console.WriteLine("Print Stream: {0}", stringFromStream);
                }
            }
            
        }

        public static void TestParseDirectory()
        {
            //File parsting test
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var rDir = EDNReader.EDNReaderFuncs.parseDirectory("..\\..\\..\\TestData\\hierarchy-noisy");
            sw.Stop();

            Console.WriteLine(String.Format("Elapsed MS: {0}", sw.ElapsedMilliseconds));
        }


        const string TEST_STR_SmallHierarchy = "[\n:pre/asdf [1 2 3 \"Asdfsaf\" 5 6 7 #{\"a\" 1 2 [7 8 9] nil} {[1 2] #{78 12} \"asdfa\" 4} foo]]";
    }
}
