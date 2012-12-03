
namespace EDNReader

module TypeHandlers =
    open EDNReader.EDNParserTypes
    open EDNTypes

    let rec defaultHandlerFuncMap (taggedHandlerFn : (QualifiedSymbol * EDNValueParsed) -> System.Object) (value : EDNValueParsed) =
        try
            match value.ednValue with
            | EDNNil -> null

            | EDNInteger bigInt -> 
                if bigInt.CompareTo(System.Int64.MaxValue) <= 0 then
                    (System.Numerics.BigInteger.op_Explicit(bigInt) : int64) :> System.Object
                else
                    bigInt :> System.Object

            | EDNString s -> s :> System.Object

            | EDNBoolean b -> b :> System.Object

            | EDNCharacter  c -> c :> System.Object
 
            | EDNFloat f -> f :> System.Object

            | EDNList l -> new EDNList (Seq.filter isNotCommentOrDiscard l |> Seq.map (defaultHandlerFuncMap taggedHandlerFn)) :> System.Object

            | EDNVector v -> new EDNVector (Array.filter isNotCommentOrDiscard v |> Array.map (defaultHandlerFuncMap taggedHandlerFn)) :> System.Object

            | EDNSet l -> 
                let filteredSeq = Seq.filter isNotCommentOrDiscard l
                let newSeq = Seq.map (fun v -> (defaultHandlerFuncMap taggedHandlerFn v)) filteredSeq
                new EDNSet(newSeq) :> System.Object

            | EDNMap l -> 
                let newSeq = Seq.filter isNotCommentOrDiscard l
                                |> Seq.map (fun v -> (defaultHandlerFuncMap taggedHandlerFn v))
                new EDNMap(newSeq) :> System.Object

            | EDNSymbol s -> new EDNSymbolType(s.prefix, s.name) :> System.Object

            | EDNKeyword k -> new EDNKeywordType(k.prefix, k.name) :> System.Object

            | EDNTaggedValue (s, v) -> taggedHandlerFn (s, v)

            | _ -> raise (System.Exception("Not an EDNValue " + value.ednValue.ToString()))
        with
        | :? EDNException as ex -> raise ex
        | ex -> raise (EDNException(ex.Message + " " + getLineColString value))

    and defaultTagHandler ((tag : QualifiedSymbol), (value : EDNValueParsed)) =
        match tag.name with
        | "uuid" when tag.prefix = null -> 
            match value.ednValue with  
            | EDNString uuidString -> new System.Guid(uuidString) :> System.Object
            | _ -> raise (System.Exception (sprintf "%A is not a valid uuid." value.ednValue))
        | "inst" when tag.prefix = null ->
            match value.ednValue with
            | EDNString dateString -> System.DateTime.Parse(dateString) :> System.Object
            | _ -> raise (System.Exception (sprintf "%A is not a valid inst." value.ednValue))
        | _ -> defaultHandlerFuncMap defaultTagHandler value
    