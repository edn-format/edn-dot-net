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
            TestParser();
            TestEquality();
            TestSampleCustomHandler();
            TestreadString();
            TestreadFile();
            TestWriter();
            TestParseDirectory();

        }

        public static void TestSampleCustomHandler()
        {
            var r1 = EDNReader.EDNReaderFuncs.readString("[1 2 {:a 3 1.2 \\c {1 2} 4}]", new SampleCustomHandler());
        }

        public static void TestreadString()
        {
            var r1 = EDNReader.EDNReaderFuncs.readString(TEST_STR_SmallHierarchy);
        }

        public static void TestEquality()
        {
            var r1 = EDNReader.EDNReaderFuncs.readString(TEST_STR_SmallHierarchy).First();
            var r2 = EDNReader.EDNReaderFuncs.readString(TEST_STR_SmallHierarchy).First();
            Funcs.Assert(r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.readString("[[nil 1 nil]]").First();
            r2 = EDNReader.EDNReaderFuncs.readString("[[1 nil 1]]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.readString("[[nil 1 nil]]").First();
            r2 = EDNReader.EDNReaderFuncs.readString("[[1 nil]]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.readString("[{nil 1 1 nil}]").First();
            r2 = EDNReader.EDNReaderFuncs.readString("[{nil 1 1 2}]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.readString("[(nil 1 1 2)]").First();
            r2 = EDNReader.EDNReaderFuncs.readString("[(4 1 1 nil)]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.readString("[#{nil 1 2 3}]").First();
            r2 = EDNReader.EDNReaderFuncs.readString("[#{nil 1 2 3}]").First();
            Funcs.Assert(r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.readString("[#{nil 1 2 3}]").First();
            r2 = EDNReader.EDNReaderFuncs.readString("[#{5 nil 1 6}]").First();
            Funcs.Assert(!r1.Equals(r2));

            r1 = EDNReader.EDNReaderFuncs.readFile("..\\..\\..\\TestData\\hierarchical.edn");

            r2 = EDNReader.EDNReaderFuncs.readFile("..\\..\\..\\TestData\\hierarchical.edn");

            Funcs.Assert(r1.Equals(r2));
        }

        public static void TestreadFile()
        {
            var r1 = EDNReader.EDNReaderFuncs.readFile("..\\..\\..\\TestData\\hierarchical.edn");
        }

        public static void TestWriter()
        {
            var r1 = EDNReader.EDNReaderFuncs.readString(TEST_STR_SmallHierarchy);

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
            var rDir = EDNReader.EDNReaderFuncs.readDirectory("..\\..\\..\\TestData\\hierarchy-noisy");
            sw.Stop();

            Console.WriteLine(String.Format("Elapsed MS: {0}", sw.ElapsedMilliseconds));
        }

        public static void TestParser()
        {
            var ast = EDNParser.EDNParserFuncs.parseFile("..\\..\\..\\TestData\\hierarchical.edn");
        }


        const string TEST_STR_SmallHierarchy = 
            @"[;comment 1
                :pre/asdf 
                    [1 2 3 ""Asdfsaf"" 5 6 7 #{""a"" 1 2 [7 8 9], nil} {[1 2] ,#{78 12} nil 1 
                    {#_ 1 nil :value} 3 ;comment 2
                    
        ,, ""asdfa"" 4 :foo nil} foo #inst ""1999-03-11T23:15:36.11Z""]]";
    }
}
