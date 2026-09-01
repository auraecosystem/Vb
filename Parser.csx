using Antlr4.Runtime;
using System;

class Program
{
    static void Main()
    {
        string vbCode = @"
Imports System

Module Program
    Sub Main()
        Dim x As Integer
        x = 42
        PrintMessage()
    End Sub

    Function GetGreeting() As String
        Return ""Hello, World!""
    End Function
End Module
";

        // Setup ANTLR Pipeline
        ICharStream inputStream = CharStreams.fromString(vbCode);
        VbLexer lexer = new VbLexer(inputStream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        VbParser parser = new VbParser(tokens);

        // Parse starting from root rule
        VbParser.CompilationUnitContext tree = parser.compilationUnit();

        // Run custom AST visitor
        var visitor = new VbAstVisitor();
        visitor.Visit(tree);
    }
}
