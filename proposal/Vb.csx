using Antlr4.Runtime.Tree;
using System;

public class SymbolTableVisitor : VbBaseVisitor<object>
{
    public Scope GlobalScope { get; } = new Scope("Global");
    private Scope _currentScope;

    public SymbolTableVisitor()
    {
        _currentScope = GlobalScope;
    }

    // Scope: Module
    public override object VisitModuleDeclaration(VbParser.ModuleDeclarationContext context)
    {
        string moduleName = context.IDENTIFIER().GetText();
        EnterScope($"Module:{moduleName}");

        base.VisitModuleDeclaration(context);

        ExitScope();
        return null;
    }

    // Scope: Subroutine
    public override object VisitSubDeclaration(VbParser.SubDeclarationContext context)
    {
        string subName = context.IDENTIFIER().GetText();
        EnterScope($"Sub:{subName}");

        base.VisitSubDeclaration(context);

        ExitScope();
        return null;
    }

    // Scope: Function (declares function parameters & returns)
    public override object VisitFunctionDeclaration(VbParser.FunctionDeclarationContext context)
    {
        string funcName = context.IDENTIFIER().GetText();
        string returnType = context.typeRef().GetText();

        // Define function in the outer scope
        _currentScope.Define(new Symbol(funcName, returnType));

        EnterScope($"Function:{funcName}");

        base.VisitFunctionDeclaration(context);

        ExitScope();
        return null;
    }

    // Declaration: Parameters
    public override object VisitParameter(VbParser.ParameterContext context)
    {
        string paramName = context.IDENTIFIER().GetText();
        string paramType = context.typeRef().GetText();

        if (!_currentScope.Define(new Symbol(paramName, paramType)))
        {
            Console.WriteLine($"[Error] Duplicate parameter definition: '{paramName}' in scope '{_currentScope.ScopeName}'");
        }

        return base.VisitParameter(context);
    }

    // Declaration: Dim x As Type
    public override object VisitFieldDeclaration(VbParser.FieldDeclarationContext context)
    {
        string varName = context.IDENTIFIER().GetText();
        string varType = context.typeRef().GetText();

        if (!_currentScope.Define(new Symbol(varName, varType)))
        {
            Console.WriteLine($"[Error] Redefinition of variable '{varName}' in scope '{_currentScope.ScopeName}'");
        }

        return base.VisitFieldDeclaration(context);
    }

    // Usage: Check symbol resolution on assignments
    public override object VisitAssignmentStatement(VbParser.AssignmentStatementContext context)
    {
        string varName = context.IDENTIFIER().GetText();
        Symbol resolved = _currentScope.Resolve(varName);

        if (resolved == null)
        {
            Console.WriteLine($"[Error] Identifier '{varName}' is undeclared in scope '{_currentScope.ScopeName}'");
        }
        else
        {
            Console.WriteLine($"[Resolved] '{varName}' -> type '{resolved.Type}'");
        }

        return base.VisitAssignmentStatement(context);
    }

    // Helper methods for scope stack management
    private void EnterScope(string scopeName)
    {
        var newScope = new Scope(scopeName, _currentScope);
        _currentScope.Children.Add(newScope);
        _currentScope = newScope;
    }

    private void ExitScope()
    {
        _currentScope = _currentScope.Parent;
    }
}
