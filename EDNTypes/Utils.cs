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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EDNTypes
{
    public static class Utils
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

        //Modified from Microsoft.CSharp.CSharpCodeGenerator source
        public static string ToLiteral(string value)
        {
            StringBuilder b = new StringBuilder(value.Length + 5);

            b.Append("\"");

            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '\r':
                        b.Append("\\r");
                        break;
                    case '\t':
                        b.Append("\\t");
                        break;
                    case '\"':
                        b.Append("\\\"");
                        break;
                    case '\\':
                        b.Append("\\\\");
                        break;
                    case '\n':
                        b.Append("\\n");
                        break;
                    default:
                        b.Append(value[i]);
                        break;
                }
            }

            b.Append("\"");

            return b.ToString();
        }
        public static void WriteEDNToStream(string edn, Stream stream)
        {
            var bytes = Encoding.UTF8.GetBytes(edn);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static string WritePrintableToString(IEDNWritable printable, IWriteHandler handler)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                printable.WriteEDN(ms, handler);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }

        public static string getSymbolString(string prefix, string name)
        {
            if (System.String.IsNullOrWhiteSpace(prefix))
                return name;
            else
                return prefix + "/" + name;
        }

        public static int additionHashCode(IEnumerable coll)
        {
            int hash = 0;
            foreach (var obj in coll)
            {
                hash = hash + (obj == null ? 0 : obj.GetHashCode());
            }
            return hash;
        }
    }
}
