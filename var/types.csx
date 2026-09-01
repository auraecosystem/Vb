using System.Collections.Generic;

// Represents a variable, parameter, or function symbol
public class Symbol
{
    public string Name { get; }
    public string Type { get; }

    public Symbol(string name, string type)
    {
        Name = name;
        Type = type;
    }
}

// Represents a lexical scope container
public class Scope
{
    public string ScopeName { get; }
    public Scope Parent { get; }
    public Dictionary<string, Symbol> Symbols { get; } = new(System.StringComparer.OrdinalIgnoreCase);
    public List<Scope> Children { get; } = new();

    public Scope(string scopeName, Scope parent = null)
    {
        ScopeName = scopeName;
        Parent = parent;
    }

    // Insert a symbol in current scope
    public bool Define(Symbol symbol)
    {
        if (Symbols.ContainsKey(symbol.Name))
            return false; // Redeclaration error

        Symbols[symbol.Name] = symbol;
        return true;
    }

    // Look up a symbol recursively up the scope chain
    public Symbol Resolve(string name)
    {
        if (Symbols.TryGetValue(name, out var symbol))
            return symbol;

        return Parent?.Resolve(name);
    }
}
