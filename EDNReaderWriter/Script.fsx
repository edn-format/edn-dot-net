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

namespace EDNReader

//Evaluate the following block to load everything you need into the F# interactive
//START
#I "C:\\dev\\edn-dot-net\\EDNReaderWriter\\bin\\Debug";;
#I "C:\\dev\\edn-dot-net\\EDNReaderTestCS\\bin\\Debug";;
#r "FParsec";;
#r "FParsecCS";;
#r "EDNTypes";;
#r "EDNReaderTestCS";;

open FParsec;;

#load "C:\\dev\\edn-dot-net\\EDNReaderWriter\\EDNParserTypes.fs";;
open EDNReaderWriter.EDNParserTypes;;
#load "C:\\dev\\edn-dot-net\\EDNReaderWriter\\TypeHandlers.fs";;
open EDNReaderWriter.TypeHandlers;;
#load "C:\\dev\\edn-dot-net\\EDNReaderWriter\\WriteHandlers.fs";;
open EDNReaderWriter.WriteHandlers;;
#load "C:\\dev\\edn-dot-net\\EDNReaderWriter\\EDNParser.fs";;
open EDNReaderWriter.EDNParser;;
#load "C:\\dev\\edn-dot-net\\EDNReaderWriter\\EDNReader.fs";;
open EDNReaderWriter.EDNReader;;
#load "C:\\dev\\edn-dot-net\\EDNReaderWriter\\EDNWriter.fs";;
open EDNReaderWriter.EDNWriter;;
//END



// Tests
(*

run (many1 parseValue) "#tag 1"  |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;

runParserOnFile (many1 parseValue) () "C:\\dev\\edn-test-data\\floats.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;

runParserOnFile (many1 parseValue) () "C:\\dev\\edn-test-data\\numbers.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) 

runParserOnFile (many1 parseValue) () "C:\\dev\\edn-test-data\\hierarchical.edn" System.Text.Encoding.UTF8 |> getValueFromResult |> List.map (defaultHandlerFuncMap defaultTagHandler) ;;
*)