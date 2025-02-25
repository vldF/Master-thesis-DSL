namespace Codegen.IR.nodes.statements;

public record CgDirectiveStatement(string Name, string Argument) : ICgStatement;
