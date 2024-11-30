namespace Codegen.IR.nodes.expressions;

public record CgVarExpression(string Name, bool isOutVar = false) : ICgExpression;
