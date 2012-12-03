﻿namespace EDNReader
module EDNReader =
    open System.Text.RegularExpressions
    open System.Numerics
    open System.IO
    open FParsec
    open EDNReader.EDNParserTypes
    open EDNReader.EDNParser
    open EDNReader.TypeHandlers

    let parseString str = 
        run (many1 parseValue) str |> getValueFromResult |> List.filter isNotCommentOrDiscard |> List.map (defaultHandlerFuncMap defaultTagHandler)

    let parseStream stream = 
        runParserOnStream (many1 parseValue) () "ednStream" stream System.Text.Encoding.UTF8 |> getValueFromResult |> List.filter isNotCommentOrDiscard |> List.map (defaultHandlerFuncMap defaultTagHandler)

    let parseFile fileName = 
        runParserOnFile (many1 parseValue) () fileName System.Text.Encoding.UTF8 |> getValueFromResult |> List.filter isNotCommentOrDiscard |> List.map (defaultHandlerFuncMap defaultTagHandler)

    let parseDirectory dir = 
        let searchPattern = @"*.edn"
        let testFiles = Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories)
        let results = [for f in testFiles do yield! parseFile f]
        results

    let tryParseDirectory dir = 
        let searchPattern = @"*.edn"
        let testFiles = Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories)
        let results = [for f in testFiles do yield!
                                                try
                                                    parseFile f
                                                with
                                                    | exn -> [printf "error parsing file %s \n" f] ]
        results
    