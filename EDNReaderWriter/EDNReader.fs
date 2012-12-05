namespace EDNReaderWriter
module EDNReader =
    open System.Text.RegularExpressions
    open System.Numerics
    open System.IO
    open FParsec
    open EDNReaderWriter.EDNParserTypes
    open EDNReaderWriter.EDNParser
    open EDNReaderWriter.TypeHandlers

    let defaultHandler = new EDNReaderWriter.TypeHandlers.BaseTypeHandler()

    type public EDNReaderFuncs =
        static member parseString str = EDNReaderFuncs.parseString(str, defaultHandler)

        static member parseString(str, (handler : BaseTypeHandler))= 
            run (many1 parseValue) str |> getValueFromResult |> List.filter isNotCommentOrDiscard |> List.map handler.handleValue

        static member parseStream stream = EDNReaderFuncs.parseStream(stream, defaultHandler)

        static member parseStream(stream, (handler : BaseTypeHandler)) = 
            runParserOnStream (many1 parseValue) () "ednStream" stream System.Text.Encoding.UTF8 |> getValueFromResult 
                |> List.filter isNotCommentOrDiscard |> List.map handler.handleValue

        static member parseFile fileName = EDNReaderFuncs.parseFile(fileName, defaultHandler)

        static member parseFile(fileName, (handler : BaseTypeHandler)) = 
            runParserOnFile (many1 parseValue) () fileName System.Text.Encoding.UTF8 |> getValueFromResult 
                |> List.filter isNotCommentOrDiscard |> List.map handler.handleValue

        static member parseDirectory dir = EDNReaderFuncs.parseDirectory(dir, defaultHandler)

        static member parseDirectory(dir, (handler : BaseTypeHandler))  = 
            let searchPattern = @"*.edn"
            let testFiles = Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories)
            let results = [for f in testFiles do yield! EDNReaderFuncs.parseFile(f, handler)]
            results

        static member tryParseDirectory dir = EDNReaderFuncs.tryParseDirectory(dir, defaultHandler) 

        static member tryParseDirectory(dir, (handler : BaseTypeHandler))  = 
            let searchPattern = @"*.edn"
            let testFiles = Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories)
            let results = [for f in testFiles do yield!
                                                    try
                                                        EDNReaderFuncs.parseFile(f, handler)
                                                    with
                                                        | exn -> [printf "error parsing file %s \n" f] ]
            results
    