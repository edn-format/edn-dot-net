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

module EDNWriter =
    open EDNReaderWriter.WriteHandlers
    open EDNTypes
    open System.IO;

    let defaultWriteHandler = new EDNReaderWriter.WriteHandlers.BaseWriteHandler()

    type public EDNWriterFuncs = 
        static member writeString obj = EDNWriterFuncs.writeString(obj, defaultWriteHandler)

        static member writeString(obj, (handler : IWriteHandler)) =
            use ms = new MemoryStream()
            handler.handleObject(obj, ms)
            ms.Position <- int64(0)
            let sr = new StreamReader(ms)
            sr.ReadToEnd()

        static member writeStream (obj, stream) = EDNWriterFuncs.writeStream(obj, stream, defaultWriteHandler)

        static member writeStream(obj, stream, (handler : IWriteHandler)) = 
            handler.handleObject(obj, stream)
        

