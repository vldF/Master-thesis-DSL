namespace Codegen.IR.nodes.expressions;

public record CgNewExpression(string TypeName, IReadOnlyCollection<ICgExpression> Args) : ICgExpression;
