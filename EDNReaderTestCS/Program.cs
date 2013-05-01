//   Copyright (c) Thortech Solutions, LLC. All rights reserved.
//   The use and distribution terms for this software are covered by the
//   Eclipse Public License 1.0 (http://opensource.org/licenses/eclipse-1.0.php)
//   which can be found in the file epl-v10.html at the root of this distribution.
//   By using this software in any fashion, you are agreeing to be bound by
//   the terms of this license.
//   You must not remove this notice, or any other, from this software.
//
//   Authors: Mark Perrotta, Dimitrios Kapsalis
//


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
            TestCustomHandler();
            TestCollections();
        }

        public static void TestSampleCustomHandler()
        {
            var r1 = EDNReader.EDNReaderFuncs.readString("[1 2 {:a 3 1.2 \\c {1 2} 4}]", new SampleCustomHandler());
        }

        public static void TestreadString()
        {
            var r1 = EDNReader.EDNReaderFuncs.readString(TEST_STR_SmallHierarchy);
        }

        public static void TestCustomHandler()
        {
            SampleCustomHandler customHandler = new SampleCustomHandler();
            SampleCustomWriter customWriter = new SampleCustomWriter();
            var r1 = EDNReader.EDNReaderFuncs.readString(TEST_STR_CustomTypeTimezone, customHandler).First();
            string printedObj = EDNWriter.EDNWriterFuncs.writeString(r1, customWriter);
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

            string s = EDNWriter.EDNWriterFuncs.writeString(r1.First());

            var r2 = EDNReader.EDNReaderFuncs.readString(s);

            Funcs.Assert(r1.Equals(r2));
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

                    var stream = EDNWriter.EDNWriterFuncs.writeStream(ednObj, ms);

                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var stringFromStream = sr.ReadToEnd();
                    Funcs.Assert(teststr.Equals(stringFromStream));
                    Funcs.Assert(ReferenceEquals(ms, stream));
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

            string writtenFirst = EDNWriter.EDNWriterFuncs.writeString(rDir.First());

            var readAgain = EDNReader.EDNReaderFuncs.readString(writtenFirst);

            Funcs.Assert(readAgain.First().Equals(rDir.First()));
        }

        public static void TestParser()
        {
            var ast = EDNParser.EDNParserFuncs.parseFile("..\\..\\..\\TestData\\hierarchical.edn");
        }

        public static void TestCollections()
        {
            var testNetCollections = new List<IDictionary<string, SortedSet<int>>>()
            {
                new Dictionary<string, SortedSet<int>>()
                {
                    {"key1", new SortedSet<int>() { 1, 2, 3 }}, 
                    {"key2", new SortedSet<int>() { 4, 5, 6 }}, 
                },

                new Dictionary<string, SortedSet<int>>()
                {
                    {"key3", new SortedSet<int>() { 1, 2, 3 }}, 
                    {"key4", new SortedSet<int>() { 4, 5, 6 }}, 
                }
            };


            string s = EDNWriter.EDNWriterFuncs.writeString(testNetCollections);

            var testNetRet = EDNReader.EDNReaderFuncs.readString(s);

            var firstDict = (IDictionary<object, object>)((IEnumerable<object>)testNetRet.First()).First();

            foreach (var kvp in firstDict)
            {
                Funcs.Assert(kvp.Value is EDNSet);
            }

            Funcs.Assert(firstDict.Contains(new KeyValuePair<object, object>("key1", new EDNSet(new object[] {1L, 2L, 3L }))));
            Funcs.Assert(firstDict.Count() == 2);

            var testMap = new EDNMap(new List<object>()
                {
                    null, new SortedSet<int>() { 1, 2, 3 },
                    "key6", new SortedSet<int>() { 4, 5, 6 },
                    "key7", 1
                });

            foreach (var kvp in testMap)
            {
                Funcs.Assert(kvp.Value != null && kvp.Value != kvp.Key);
            }

            Funcs.Assert(testMap[null] is SortedSet<int>);
            Funcs.Assert(testMap.Count() == 3);
            Funcs.Assert(testMap.Where(kvp => (string)kvp.Key == "key7").Count() == 1);
            Funcs.Assert(testMap.Select(kvp => (string)kvp.Key == "key7").First() == false);
            Funcs.Assert(testMap.ContainsKey(null));
            Funcs.Assert(testMap.ContainsKey("key6"));
            Funcs.Assert(testMap.ContainsKey("missingkey") == false);

            Funcs.Assert(testMap.Keys.Count() == testMap.Values.Count());
            Funcs.Assert(testMap.Values.Count() == 3);
            Funcs.Assert(testMap.Values.Contains(1));
            Funcs.Assert(testMap.Values.Contains(999) == false);
            Funcs.Assert(testMap.Keys.Contains(null));
            Funcs.Assert(testMap.Keys.Contains("key6"));

            Funcs.Assert(testMap.ToList().Count(kvp => kvp.Key == null) == 1);

            //test array
            s = EDNWriter.EDNWriterFuncs.writeString(testNetCollections.ToArray());
            testNetRet = EDNReader.EDNReaderFuncs.readString(s);

            Funcs.Assert(testNetRet.First() is EDNVector);

            //ET Test using KeyValue pairs outside of dictionaties.
            var lstkv = new List<KeyValuePair<Int32, string[]>>()
                {
                    new KeyValuePair<int, string[]>(1, new string[] {"fred", "wilma"})
                    ,
                    new KeyValuePair<int, string[]>(2, new string[] {"again", "whatever"})
                };
            s = EDNWriter.EDNWriterFuncs.writeString(lstkv);
            testNetRet = EDNReader.EDNReaderFuncs.readString(s);
            //ET Make sure all the items in the list are EDNVectors
            Funcs.Assert(testNetRet.First() is EDNList
                && ((EDNList)testNetRet.First()).All(l => l is EDNVector));
            // ET Each EDNVector should have a list with pairs where the 1st element is an int and the second is a EDNVector of strings
            var vecenum = ((EDNList) testNetRet.First())
                .Cast<EDNVector>()
                .Select(x => x.Select((v, i) => new {Index = i, Value = v})
                                     .GroupBy(a => a.Index/2)
                                     .Select(y => y.Select(v => v.Value).ToList()))
                .Select(mlst => mlst.All(lst => lst[0] is Int32 && lst[1] is EDNVector && lst.All(d=>d is String)))
                .All(y=>y);
        }


        const string TEST_STR_SmallHierarchy = 
            @"[;comment 1
                :pre/asdf 
                    [1 2 3 ""Asdfsaf"" 5 6 7 #{""a"" 1 2 [7 8 9], nil} {[1 2] ,#{78 12} nil 1 
                    {#_ 1 nil :value} 3 ;comment 2
                    
        ,, ""asdfa"" 4 :foo nil} foo #inst ""1999-03-11T23:15:36.11Z""]]";

        /// <summary>
        /// A sample type (System.TimeZoneInfo) to test the custom handler.
        /// </summary>
        const string TEST_STR_CustomTypeTimezone =
            "#sample-custom-type/timezone {:Id \"Custom Time Zone\" :BaseUtcOffset \"02:00:00\" :DisplayName \"(UTC+02:00) Custom Time Zone\" :StandardDisplayName \"Custom Time Zone\"}";
    }
}
