namespace Codegen.IR.nodes.expressions;

public class CgStringExpression(string value) : ICgExpression
{
    public string Value { get; } = "\"" + value + "\"";
}
