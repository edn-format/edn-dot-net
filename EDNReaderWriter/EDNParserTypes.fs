
namespace EDNReaderWriter
open System.Numerics
open EDNTypes

module EDNParserTypes =
    type EDNException(message : string) = 
        inherit System.Exception(message)
        
    type QualifiedSymbol = 
        struct
            val prefix: string
            val name: string
            new (prefix, name) = {prefix = prefix; name = name}
            override this.ToString() = "QualifiedSymbol Prefix: " + this.prefix + " Name: " + this.name 
        end

    type EDNValue = EDNNil
                    | EDNBoolean of bool
                    | EDNString of string
                    | EDNCharacter of char
                    | EDNSymbol of QualifiedSymbol
                    | EDNKeyword of QualifiedSymbol
                    | EDNInteger of BigInteger
                    | EDNFloat of double
                    | EDNComment of string
                    | EDNDiscard of EDNValueParsed
                    | EDNTaggedValue of QualifiedSymbol * EDNValueParsed
                    | EDNList of EDNValueParsed list
                    | EDNVector of EDNValueParsed array
                    | EDNMap of List<EDNValueParsed>
                    | EDNSet of List<EDNValueParsed>
    and EDNValueParsed = 
        struct
            val line: int64
            val col: int64
            val ednValue: EDNValue
            new (ednValue, line, col) = { ednValue = ednValue; line = line; col = col }
            override this.ToString() = 
                sprintf "%A" this.ednValue
        end

    let getLineColString (valueParsed : EDNValueParsed) =
        System.String.Format("line: {0}, column: {1}", valueParsed.line, valueParsed.col); 
        
    let isNotCommentOrDiscard (v : EDNValueParsed) =
        match v.ednValue with 
        | EDNComment _ | EDNDiscard _ -> false
        | _ -> true
        (*
    type IEDNSymbol =
        inherit System.IComparable
        abstract member getPrefix : unit -> string
        abstract member getName : unit -> string

    let getSymbolString (symbol : IEDNSymbol )=
        if System.String.IsNullOrWhiteSpace(symbol.getPrefix()) then 
                System.String.Empty 
        else symbol.getPrefix() + "/" 
        + symbol.getName()

    type EDNSymbolType(prefix: string, name: string) = 
        interface IEDNPrintable with
            member this.PrintEDN(stream) =
                PrintUtils.WriteEDNToStream(this.ToString(), stream)
            member this.PrintEDN() = 
                this.ToString()
        interface IEDNSymbol with
            member this.getPrefix() = prefix
            member this.getName() = name
            member this.CompareTo obj = 
                this.ToString().CompareTo( obj.ToString())
        override this.ToString() = getSymbolString this
        override this.GetHashCode() = 
            this.ToString().GetHashCode()
        override this.Equals (obj : System.Object) = 
            if System.Object.ReferenceEquals(this, obj) then
                true
            elif obj.GetType() <> Operators.typeof<EDNSymbolType> then
                false
            elif this.ToString() = obj.ToString() then
                true
            else
                false

    type EDNKeywordType(prefix: string, name: string) =
        interface IEDNPrintable with
            member this.PrintEDN(stream) =
                PrintUtils.WriteEDNToStream(":" + this.ToString(), stream)
            member this.PrintEDN() = 
                ":" + this.ToString()
        interface IEDNSymbol with
            member this.getPrefix() = prefix
            member this.getName() = name
            member this.CompareTo obj = 
                this.ToString().CompareTo( obj.ToString())
        override this.ToString() = getSymbolString this
        override this.GetHashCode() = 
            this.ToString().GetHashCode()
        override this.Equals (obj : System.Object) = 
            if System.Object.ReferenceEquals(this, obj) then
                true
            elif obj.GetType() <> Operators.typeof<EDNKeywordType> then
                false
            elif this.ToString() = obj.ToString() then
                true
            else
                false
                *)