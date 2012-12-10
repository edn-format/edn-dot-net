
# edn-dot-net
EDN Library for .NET
# Summary
This library provides the following [EDN](https://github.com/edn-format/edn) support for .NET:  
* EDN Reader/Parser: Access to the EDN abstract syntax tree(AST)  
* EDN Serialization: An extensible framework for EDN serialization and deserialization  

# Why not use clojure.clr?
Why not use the clojure.clr instead of edn-dot-net ?
  
The ability to customize serialization and deserilization, e.g.:  
  * You might want your EDN ints to be int32 instead of BigInts  
  * Implement serialization/deserialization for you own custom types  


# Deserialization
## EDNReaderWriter.EDNReader.EDNReaderFuncs  
### Overview
The reader functions below return an object graph created from the EDN data. Each function has two arities: one which uses the default type handler and one which
allows you to pass in your own custom type handler. A sample custom type handler can be found in the EDNReaderTestCS project (SampleCustomHandler).

Note: the functions which return lists of objects do so because the EDN data may contain more than one "root" EDN element. In the case of a stream only the 
first/next object is returned. 

* readString(string edn) - Takes an EDN string and returns a list of object graphs.
* readFile(string fileName) - Takes a path to an EDN file and returns a list of object graphs.  
* readDirectory(string directoryPath) - Takes a directory path and reads all files with extension ".edn". Returns a list of objects graphs.
* readStream(Stream charStream) - Takes a stream of characters and reads the first/next EDN element. Returns a single object graph.

### Default type handler
The default type handler is consistent with the equality rules of EDN.  For collections it uses types which have
EDN equality symantics. 

EDN Type	-> .NET Type
----------------------------------
nil			-> null
integer		-> Int64 or System.Numerics.BigInteger (if too large to fit in Int64)
string		-> string
bool		-> bool
float		-> double
list		-> EDNTypes.EDNList
vector		-> EDNTypes.EDNVector
set			-> EDNTypes.EDNSet
list		-> EDNTypes.EDNMap
symbol		-> EDNTypes.EDNVector
keyword		-> EDNTypes.EDNSet
inst		-> DateTime
uuid		-> System.Guid 

  
## Customizing reader
### ITypeHandler
If you want to customize reading you can implement TypeHandlers.ITypeHandler or derive from the default type handler: 
TypeHandlers.BaseTypeHandler. An example of this can be found in the EDNReaderTestCS project (SampleCustomHandler).

# Serialization
## EDNReaderWriter.EDNWriter.EDNWriterFuncs  
### overview
The writer functions below take an object graph and convert it to EDN character data. Each function has two arities: one which uses the default print handler 
and one which allows you to pass in your own custom print handler. A sample custom print handler can be found in the EDNReaderTestCS project (SampleCustomPrinter).

* writeString - Takes an object graph and returns a string of EDN data. 
* writeStream - Takes an object graph and writes EDN character data to a stream.

### Default print handler
The default print handler is the inverse of the default type handler in the reader (see above)

## Customizing writing
### IEDNPrintable
If you have your own custom types, you can derive from IEDNPrintable and implement the two PrintEDN functions 

### IPrintHandler
If you want to customize printing for types that cannot implement IEDNPrintable, you can implement IPrintHandler or derive 
from the default print handler: PrintHandlers.BasePrintHandler. An example of this can be found in the EDNReaderTestCS project (SampleCustomPrinter).

# Parser  
The parsing functionality is exposed if you want to work directly with the EDN syntax tree.

## EDNReaderWriter.EDNParser.EDNParserFuncs  
Here is where you will find conveniant funtions for generating syntax trees for EDN data  
* parseString - doc  
* parseFile - doc  
* parseDirectory - doc  
* parseStream - doc  


# Examples
Examples for CSharp can be found in the EDNReaderTestCS projects. F# examples can ve found in thge EDNReaderTest project.