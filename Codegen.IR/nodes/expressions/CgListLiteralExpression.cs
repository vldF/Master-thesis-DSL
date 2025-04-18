namespace Codegen.IR.nodes.expressions;

public record CgListLiteralExpression(IReadOnlyCollection<ICgExpression> elements) : ICgExpression;
