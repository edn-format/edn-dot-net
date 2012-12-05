namespace EDNReaderWriter

module EDNWriter =
    open EDNReaderWriter.PrintHandlers
    open EDNTypes
    open System.IO;

    let defaultWriteHandler = new EDNReaderWriter.PrintHandlers.BasePrintHandler()

    type public EDNWriterFuncs = 
        static member writeString obj = EDNWriterFuncs.writeString(obj, defaultWriteHandler)

        static member writeString(obj, (handler : IPrintHandler)) =
            use ms = new MemoryStream()
            handler.handleObject(obj, ms)
            ms.Position <- int64(0)
            let sr = new StreamReader(ms)
            sr.ReadToEnd()

        static member writeStream (obj, stream) = EDNWriterFuncs.writeStream(obj, stream, defaultWriteHandler)

        static member writeStream(obj, stream, (handler : IPrintHandler)) = 
            handler.handleObject(obj, stream)
        

