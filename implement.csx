using Antlr4.Runtime.Tree;
using System;

public class VbAstVisitor : VbBaseVisitor<object>
{
    // Override root visitor entry
    public override object VisitCompilationUnit(VbParser.CompilationUnitContext context)
    {
        Console.WriteLine("--- Starting AST Traversal ---");
        return base.VisitCompilationUnit(context);
    }

    // Visit Subroutine Declarations
    public override object VisitSubDeclaration(VbParser.SubDeclarationContext context)
    {
        string subName = context.IDENTIFIER().GetText();
        Console.WriteLine($"Found Subroutine: {subName}");

        // Continue visiting child statements inside the Sub
        return base.VisitSubDeclaration(context);
    }

    // Visit Function Declarations
    public override object VisitFunctionDeclaration(VbParser.FunctionDeclarationContext context)
    {
        string funcName = context.IDENTIFIER().GetText();
        string returnType = context.typeRef().GetText();
        Console.WriteLine($"Found Function: {funcName} -> Return Type: {returnType}");

        return base.VisitFunctionDeclaration(context);
    }

    // Visit Variable Assignments
    public override object VisitAssignmentStatement(VbParser.AssignmentStatementContext context)
    {
        string varName = context.IDENTIFIER().GetText();
        string value = context.expression().GetText();
        Console.WriteLine($"  Assignment: {varName} = {value}");

        return base.VisitAssignmentStatement(context);
    }
}
