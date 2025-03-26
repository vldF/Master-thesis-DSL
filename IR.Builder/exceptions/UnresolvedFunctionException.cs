namespace Ast.Builder.exceptions;

public class UnresolvedFunctionException(string name) : Exception($"function {name} is unresolved") { }
