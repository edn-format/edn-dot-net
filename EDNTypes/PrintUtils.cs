using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace EDNTypes
{
    public static class PrintUtils
    {
        private static readonly CodeDomProvider cSharpProvider = CodeDomProvider.CreateProvider("CSharp");

        public static readonly Byte[] openVectorBytes = Encoding.UTF8.GetBytes("[");
        public static readonly Byte[] closeVectorBytes = Encoding.UTF8.GetBytes("]");

        public static readonly Byte[] openListBytes = Encoding.UTF8.GetBytes("(");
        public static readonly Byte[] closeListBytes = Encoding.UTF8.GetBytes(")");

        public static readonly Byte[] openMapBytes = Encoding.UTF8.GetBytes("{");
        public static readonly Byte[] closeMapBytes = Encoding.UTF8.GetBytes("}");

        public static readonly Byte[] openSetBytes = Encoding.UTF8.GetBytes("#{");
        public static readonly Byte[] closeSetBytes = Encoding.UTF8.GetBytes("}");

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this object obj, Stream stream)
        {

            throw new Exception(string.Format("Don't know how to serialize type {0} to EDN", obj.GetType().ToString()));

        }

        /// <summary>
        /// Serializes the v graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this decimal d, Stream stream)
        {
            WriteEDNToStream(d.ToString(), stream);

        }

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this double d, Stream stream)
        {

            WriteEDNToStream(d.ToString(), stream);
        }

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this float f, Stream stream)
        {
            WriteEDNToStream(f.ToString(), stream);

        }

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this Int32 i, Stream stream)
        {
            WriteEDNToStream(i.ToString(), stream);
        }

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this Int64 i, Stream stream)
        {
            WriteEDNToStream(i.ToString(), stream);
        }

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this BigInteger b, Stream stream)
        {
            WriteEDNToStream(b.ToString(), stream);
        }


        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this string s, Stream stream)
        {
            WriteEDNToStream(ToLiteral(s), stream);
        }

        /// <summary>
        /// Serializes the type graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this char c, Stream stream)
        {
            string str;
            switch(c)
            {
                case '\t': str = "\\tab";
                    break;
                case '\r': str = "\\return";
                    break;
                case '\n': str = "\\newline";
                    break;
                case ' ': str = "\\space";
                    break;
                default: 
                    str = c.ToString();
                    break;
            }
            WriteEDNToStream(str, stream);
        }

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this bool b, Stream stream)
        {
            string str = b ? "true" : "false";
            WriteEDNToStream(str, stream);
        }

        /// <summary>
        /// Serializes the object graph as a character string to a stream
        /// </summary>
        /// <param name="stream"></param>
        public static void PrintEDN(this KeyValuePair<object, object> kvp, Stream stream)
        {
            PrintEDNObjectToStream(kvp.Key, stream);
            stream.Write(spaceBytes, 0, spaceBytes.Length);
            PrintEDNObjectToStream(kvp.Value, stream);
        }

        public static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                cSharpProvider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                return writer.ToString();
            }
        }

        public static void WriteEDNToStream(string edn, Stream stream)
        {
            var bytes = Encoding.UTF8.GetBytes(edn);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static Byte[] nullBytes = Encoding.UTF8.GetBytes("nil");

        public static void PrintEDNObjectToStream(object obj, Stream stream)
        {
            if (obj == null)
            {
                stream.Write(nullBytes, 0, nullBytes.Length);
                return;
            }

            var printable = obj as IEDNPrintable;
            
            if (printable != null)
                printable.PrintEDN(stream);
            else
            {
                Type t = obj.GetType();
                if (t == typeof(Int32))
                    PrintEDN((Int32)obj, stream);
                else if (t == typeof(Int64))
                    PrintEDN((Int64)obj, stream);
                else if (t == typeof(double))
                    PrintEDN((double)obj, stream);
                else if (t == typeof(float))
                    PrintEDN((float)obj, stream);
                else if (t == typeof(decimal))
                    PrintEDN((decimal)obj, stream);
                else if (t == typeof(BigInteger))
                    PrintEDN((BigInteger)obj, stream);
                else if (t == typeof(string))
                    PrintEDN((string)obj, stream);
                else if (t == typeof(char))
                    PrintEDN((char)obj, stream);
                else if (t == typeof(bool))
                    PrintEDN((bool)obj, stream);
                else if (t.Equals(typeof(KeyValuePair<object,object>)))
                    PrintEDN((KeyValuePair<object, object>)obj, stream);
                else
                    PrintEDN(obj, stream);
            }
        }

        public static Byte[] spaceBytes = Encoding.UTF8.GetBytes(" ");

        public static void PrintIEnumerableToEDN(IEnumerable enumerable, System.IO.Stream stream)
        {
            var enumerator = enumerable.GetEnumerator();
            bool movedNext = enumerator.MoveNext();
            while(movedNext)
            {
                PrintUtils.PrintEDNObjectToStream(enumerator.Current, stream);
                movedNext = enumerator.MoveNext();
                if (movedNext)
                    stream.Write(spaceBytes, 0, spaceBytes.Length);
            }
        }

        public static string WriteStreamToString(IEDNPrintable printable)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                printable.PrintEDN(ms);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }
    }
}
