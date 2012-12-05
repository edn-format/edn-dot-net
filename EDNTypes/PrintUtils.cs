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

        public static readonly Byte[] spaceBytes = Encoding.UTF8.GetBytes(" ");

        public static readonly Byte[] nullBytes = Encoding.UTF8.GetBytes("nil");

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

        public static string WritePrintableToString(IEDNPrintable printable, IPrintHandler handler)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                printable.PrintEDN(ms, handler);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }
    }
}