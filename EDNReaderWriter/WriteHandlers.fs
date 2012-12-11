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

namespace EDNReaderWriter

module WriteHandlers = 
    open EDNTypes
    open System.IO
    open System.Collections
    open System.Collections.Generic
    open System.Numerics

    type public BaseWriteHandler() = 

        interface IWriteHandler with
            override this.handleObject (obj, stream) = this.handleObject (obj, stream)
            override this.handleEnumerable (enumerable, stream) = this.handleEnumerable (enumerable, stream)

        abstract member handleObject: System.Object * Stream -> unit
        default this.handleObject(obj, stream) =
            match obj with
            | null -> stream.Write(Utils.nullBytes, 0, Utils.nullBytes.Length)

            | :? IEDNWritable as writeable -> writeable.WriteEDN(stream, this)
                
            | :? System.Int32 | :? System.Int64 | :? System.Double
            | :? System.Single | :? System.Decimal | :? BigInteger as i -> Utils.WriteEDNToStream(i.ToString(), stream)
                
            | :? System.String as i -> Utils.WriteEDNToStream(Utils.ToLiteral(i), stream)
                
            | :? System.Char as i ->
                let str =
                    match i with
                    | '\t' -> "\\tab"
                    | '\r' -> "\\return"
                    | '\n' -> "\\newline"
                    | ' ' -> "\\space"
                    | _ -> i.ToString()
                Utils.WriteEDNToStream(str, stream)
                
            | :? System.Boolean as i ->
                let str = if i then "true" else "false"
                Utils.WriteEDNToStream(str , stream)
                
            | :? KeyValuePair<System.Object, System.Object> as kvp ->
                this.handleObject(kvp.Key, stream)
                stream.Write(Utils.spaceBytes, 0, Utils.spaceBytes.Length);
                this.handleObject(kvp.Value, stream)
                
            | :? System.Guid as i ->
                Utils.WriteEDNToStream(System.String.Format("#uuid \"{0}\"", i.ToString("D")), stream)

            | :? System.DateTime as i ->
                Utils.WriteEDNToStream(System.String.Format("#inst \"{0}\"", i.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")), stream)

            | _ -> raise (System.Exception("Cannot write edn for type " + obj.GetType().ToString()))
        
        abstract member handleEnumerable: IEnumerable * Stream -> unit
        default this.handleEnumerable (enumerable, stream) =
            let enumerator = enumerable.GetEnumerator()
            let mutable movedNext = enumerator.MoveNext()
            while movedNext do
                this.handleObject (enumerator.Current, stream)
                movedNext <- enumerator.MoveNext()
                if movedNext then
                    stream.Write(Utils.spaceBytes, 0, Utils.spaceBytes.Length)