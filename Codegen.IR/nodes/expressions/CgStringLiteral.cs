namespace Codegen.IR.nodes.expressions;

public class CgStringLiteral(string value) : ICgExpression
{
    public string Value { get; } = "\"" + value + "\"";
}
