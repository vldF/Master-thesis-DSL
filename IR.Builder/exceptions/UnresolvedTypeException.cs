namespace me.vldf.jsa.dsl.ir.builder.exceptions;

public class UnresolvedTypeException(string name) : Exception($"type {name} is unresolved") { }
