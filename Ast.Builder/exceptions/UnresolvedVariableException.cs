namespace Ast.Builder.exceptions;

public class UnresolvedVariableException(string name) : Exception($"variable {name} is unresolved") { }
