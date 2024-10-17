namespace Codegen.IR.nodes.expressions;

public record CgBinExpression(
    ICgExpression Left,
    ICgExpression Right,
    CgBinExpression.BinOp Operation) : ICgExpression
{
    public enum BinOp
    {
        Plus, Minus, Mul, Div, Eq, NotEq, Less, Great, LessEq, GreatEq,
        And, Or
    }

}
