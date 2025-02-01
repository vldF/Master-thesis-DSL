using System.Globalization;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public record IntLiteralAstNode(int Value) : IExpressionAstNode
{
    public string String()
    {
        return Value.ToString();
    }
}

public record FloatLiteralAstNode(double Value) : IExpressionAstNode
{
    public string String()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
}

public record BoolLiteralAstNode(bool Value) : IExpressionAstNode
{
    public string String()
    {
        return Value.ToString();
    }
}

public record StringLiteralAstNode(string Value) : IExpressionAstNode
{
    public string String()
    {
        return $"\"{Value}\"";
    }
}
