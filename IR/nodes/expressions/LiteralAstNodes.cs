using System.Globalization;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public record IntLiteralAstNode(int Value) : IExpressionAstNode
{
    public string String()
    {
        return Value.ToString();
    }

    public bool IsSyntetic { get; set; }
}

public record FloatLiteralAstNode(double Value) : IExpressionAstNode
{
    public string String()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public bool IsSyntetic { get; set; }
}

public record BoolLiteralAstNode(bool Value) : IExpressionAstNode
{
    public string String()
    {
        return Value.ToString();
    }

    public bool IsSyntetic { get; set; }
}

public record StringLiteralAstNode(string Value) : IExpressionAstNode
{
    public string String()
    {
        return $"\"{Value}\"";
    }

    public bool IsSyntetic { get; set; }
}
