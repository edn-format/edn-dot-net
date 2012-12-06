namespace EDNReaderWriter

module PrintHandlers = 
    open EDNTypes
    open System.IO
    open System.Collections.Generic
    open System.Numerics;

    type public BasePrintHandler() = 
        interface IPrintHandler with
            member this.handleObject (obj, stream) =
                match obj with
                | null -> stream.Write(Utils.nullBytes, 0, Utils.nullBytes.Length)

                | :? IEDNPrintable as printable -> printable.PrintEDN(stream, this)
                
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
                    (this :> IPrintHandler).handleObject(kvp.Key, stream)
                    stream.Write(Utils.spaceBytes, 0, Utils.spaceBytes.Length);
                    (this :> IPrintHandler).handleObject(kvp.Value, stream)
                
                | _ -> raise (System.Exception("Cannot write edn for type " + obj.GetType().ToString()))
        
            member this.handleEnumerable (enumerable, stream) =
                let enumerator = enumerable.GetEnumerator()
                let mutable movedNext = enumerator.MoveNext()
                while movedNext do
                    (this :> IPrintHandler).handleObject (enumerator.Current, stream)
                    movedNext <- enumerator.MoveNext()
                    if movedNext then
                        stream.Write(Utils.spaceBytes, 0, Utils.spaceBytes.Length)