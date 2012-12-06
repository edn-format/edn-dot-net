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
        static member readString str = EDNReaderFuncs.readString(str, defaultHandler)

        static member readString(str, (handler : ITypeHandler))= 
            EDNParserFuncs.parseString str |> List.filter isNotCommentOrDiscard |> List.map handler.handleValue

        static member readStream stream = EDNReaderFuncs.readStream(stream, defaultHandler)

        static member readStream(stream, (handler : ITypeHandler)) = 
            EDNParserFuncs.parseStream stream |> handler.handleValue

        static member readFile fileName = EDNReaderFuncs.readFile(fileName, defaultHandler)

        static member readFile(fileName, (handler : ITypeHandler)) = 
            EDNParserFuncs.parseFile fileName
                |> List.filter isNotCommentOrDiscard |> List.map handler.handleValue

        static member readDirectory dir = EDNReaderFuncs.readDirectory(dir, defaultHandler)

        static member readDirectory(dir, (handler : BaseTypeHandler))  = 
            let searchPattern = @"*.edn"
            let testFiles = Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories)
            let results = [for f in testFiles do yield! EDNReaderFuncs.readFile(f, handler)]
            results

        static member tryReadDirectory dir = EDNReaderFuncs.tryReadDirectory(dir, defaultHandler) 

        static member tryReadDirectory(dir, (handler : BaseTypeHandler))  = 
            let searchPattern = @"*.edn"
            let testFiles = Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories)
            let results = [for f in testFiles do yield!
                                                    try
                                                        EDNReaderFuncs.readFile(f, handler)
                                                    with
                                                        | exn -> [printf "error parsing file %s \n" f] ]
            results
    