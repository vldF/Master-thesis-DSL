namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public record BinaryExpressionAstNode(
    IExpressionAstNode Left,
    IExpressionAstNode Right,
    BinaryOperation Op
    ) : IExpressionAstNode
{
    public string String()
    {
        return $"({Left.String()} {Op.ToString()} {Right.String()})";
    }
}

public enum BinaryOperation
{
    Mul, Div, Mod, Sum, Sub, Eq, NotEq, LtEq, Lt, GtEq, Gt, AndAnd, OrOr, Xor
}
