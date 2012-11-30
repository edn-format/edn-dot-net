namespace EDNReader

//Evaluate the following block to load everything you need into the F# interactive
//START
#I "C:\\dev\\edn-dot-net\\EDNReader\\bin\\Debug";;
#r "FParsec";;
#r "FParsecCS";;
#r "EDNTypes";;

open FParsec;;

#load "C:\\dev\\edn-dot-net\\EDNReader\\EDNParserTypes.fs";;
open EDNReader.EDNParserTypes;;
#load "C:\\dev\\edn-dot-net\\EDNReader\\TypeHandlers.fs";;
open EDNReader.TypeHandlers;;
#load "C:\\dev\\edn-dot-net\\EDNReader\\EDNParser.fs";;
open EDNReader.EDNParser;;
#load "C:\\dev\\edn-dot-net\\EDNReader\\EDNReader.fs";;
open EDNReader.EDNReader;;
//END



// Tests
(*

run (many1 parseValue) "#tag 1"  |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;

runParserOnFile (many1 parseValue) () "C:\\dev\\edn-test-data\\floats.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;

runParserOnFile (many1 parseValue) () "C:\\dev\\edn-test-data\\numbers.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) 

runParserOnFile (many1 parseValue) () "C:\\dev\\edn-test-data\\hierarchical.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;
*)