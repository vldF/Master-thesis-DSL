namespace Codegen.IR.nodes.expressions;

public record CgIntLiteral(int Value) : ICgExpression
{
    public static readonly CgIntLiteral Const0 = new(0);
    public static readonly CgIntLiteral Const1 = new(1);
    public static readonly CgIntLiteral ConstNeg1 = new(-1);
}
