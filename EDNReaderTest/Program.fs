// Learn more about F# at http://fsharp.net
//#r "EDNReader";;
namespace EDNReaderTest
module test =
    open FParsec
    open EDNReaderWriter.EDNParserTypes
    open EDNReaderWriter.EDNParser
    open EDNReaderWriter.TypeHandlers

    open System.Diagnostics
    open System.IO

    let ednTestDir = @"C:\\dev\\edn-test-data"
    let ednTestDir2 = @"C:\\dev\\edn-test-data"

    let time (f : (unit -> 'a)) = 
        let stopWatch = Stopwatch.StartNew()
        let result = f ()
        stopWatch.Stop()
        printfn "%f" stopWatch.Elapsed.TotalMilliseconds
        result

    (*
    runParserOnFile (many1 parseValue) () "C:\\dev\\edn-test-data\\hierarchical.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;

    time (fun () -> 
            runParserOnFile (many1 parseValue) () 
                        "C:\\dev\\edn-test-data\\hierarchical.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler))

    run (many1 parseValue) "#{:a 1 :b 2 :b}"  |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;
    *)

    


