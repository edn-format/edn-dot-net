
# edn-dot-net
EDN Library for .NET
# Summary
This library provides the following [EDN](https://github.com/edn-format/edn) support for .NET:  
* EDN Reader/Parser: Access to the EDN abstract syntax tree(AST)  
* EDN Serialization: An extensible framework for EDN serialization and deserialization  

# Why not use clojure.clr?
Why not use the clojure.clr instead edn-dot-net ?
  
The ability to customize serialization and deserilization, e.g.:  
  * You might want your EDN ints to be int32 instead of BigInts  
  * Implement serialization/deserialization for you own custom types  



# Deserialization
## EDNReaderWriter.EDNReader.EDNReaderFuncs  
* readString - doc  
* readFile - doc  
* readDirectory - doc  
* readStream - doc  

# Serialization
## EDNReaderWriter.EDNWriter.EDNWriterFuncs  
* writeString - doc  
* writeStream - doc  

# Parser  
The parsing functionality is exposed if you want to work directly with the EDN syntax tree.

## EDNReaderWriter.EDNParser.EDNParserFuncs  
Here is where you will find conveniant funtions for generating syntax trees for EDN data  
* parseString - doc  
* parseFile - doc  
* parseDirectory - doc  
* parseStream - doc  
