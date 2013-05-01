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
            override this.handleObject (obj, stream,parent) = this.handleObject (obj, stream,parent)
            override this.handleObject (obj, stream) = this.handleObject (obj, stream,null)
            override this.handleEnumerable (enumerable, stream,parent) = this.handleEnumerable (enumerable, stream,parent)
            override this.handleEnumerable (enumerable, stream) = this.handleEnumerable (enumerable, stream,null)

        abstract member handleObject: System.Object * Stream * System.Object -> unit
        default this.handleObject(obj, stream, parent) =            
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
                    | _ -> "\\" + i.ToString()
                Utils.WriteEDNToStream(str, stream)
                
            | :? System.Boolean as i ->
                let str = if i then "true" else "false"
                Utils.WriteEDNToStream(str , stream)

            | :? System.Guid as i ->
                Utils.WriteEDNToStream(System.String.Format("#uuid \"{0}\"", i.ToString("D")), stream)

            | :? System.DateTime as i ->
                Utils.WriteEDNToStream(System.String.Format("#inst \"{0}\"", i.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")), stream)

            | :? System.Collections.IDictionary as dict ->
                stream.Write(Utils.openMapBytes, 0, Utils.openMapBytes.Length)
                this.handleEnumerable (dict, stream,obj)
                stream.Write(Utils.closeMapBytes, 0, Utils.closeMapBytes.Length)

            | :? System.Array as array ->
                stream.Write(Utils.openVectorBytes, 0, Utils.openVectorBytes.Length)
                this.handleEnumerable (array, stream,obj)
                stream.Write(Utils.closeVectorBytes, 0, Utils.closeVectorBytes.Length)

            | :? System.Collections.IList as lst ->
                stream.Write(Utils.openListBytes, 0, Utils.openListBytes.Length)
                this.handleEnumerable (lst, stream,obj)
                stream.Write(Utils.closeListBytes, 0, Utils.closeListBytes.Length)

            | _ as obj ->
                let typ = obj.GetType()
                if typ.IsGenericType then
                    let genericType = typ.GetGenericTypeDefinition()
                    // Need to come up with faster alternative to this reflection
                    if genericType.Name = "KeyValuePair`2" then
                        let key = typ.GetProperty("Key").GetValue(obj, null)
                        let value = typ.GetProperty("Value").GetValue(obj, null)
                        let indictionary = parent :? IDictionary
                        if not indictionary then stream.Write(Utils.openVectorBytes, 0, Utils.openVectorBytes.Length)
                        this.handleObject(key, stream,obj)
                        stream.Write(Utils.spaceBytes, 0, Utils.spaceBytes.Length);
                        this.handleObject(value, stream,obj)
                        if not indictionary then stream.Write(Utils.closeVectorBytes, 0, Utils.closeVectorBytes.Length)
                    elif Seq.exists (fun (t : System.Type) -> t.Name = "ISet`1") (genericType.GetInterfaces() :> seq<System.Type>)   then
                        let set = obj :?> System.Collections.IEnumerable
                        stream.Write(Utils.openSetBytes, 0, Utils.openSetBytes.Length)
                        this.handleEnumerable (set, stream,obj)
                        stream.Write(Utils.closeSetBytes, 0, Utils.closeSetBytes.Length)
                    else
                        raise (System.Exception("Cannot write edn for type " + obj.GetType().ToString()))
                else
                    raise (System.Exception("Cannot write edn for type " + obj.GetType().ToString()))
                    
        abstract member handleEnumerable: IEnumerable * Stream * System.Object ->  unit
        default this.handleEnumerable (enumerable, stream,parent) =            
            let enumerator = enumerable.GetEnumerator()
            let mutable movedNext = enumerator.MoveNext()
            while movedNext do
                this.handleObject (enumerator.Current, stream,parent)
                movedNext <- enumerator.MoveNext()
                if movedNext then
                    stream.Write(Utils.spaceBytes, 0, Utils.spaceBytes.Length)