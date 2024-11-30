namespace Codegen.IR.nodes.expressions;

public record CgFloatLiteral(double Value) : ICgExpression
{
    public static readonly CgFloatLiteral Const0 = new(0.0);
}
