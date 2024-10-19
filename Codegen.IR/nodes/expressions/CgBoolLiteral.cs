namespace Codegen.IR.nodes.expressions;

public record CgBoolLiteral(bool Value) : ICgExpression
{
    public static readonly CgBoolLiteral True = new(true);
    public static readonly CgBoolLiteral False = new(false);
}
