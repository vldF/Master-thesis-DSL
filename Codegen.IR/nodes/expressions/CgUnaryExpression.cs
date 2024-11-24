namespace Codegen.IR.nodes.expressions;

public record CgUnaryExpression(
    ICgExpression Value,
    CgUnOp Operation) : ICgExpression;

public enum CgUnOp
{
    Minus, Not,
}
