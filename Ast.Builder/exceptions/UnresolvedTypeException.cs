namespace Ast.Builder.exceptions;

public class UnresolvedTypeException(string name) : Exception($"type {name} is unresolved") { }
