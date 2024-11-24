namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public record UnaryExpressionAstNode(
    IExpressionAstNode Value,
    UnaryOperation Op
    ) : IExpressionAstNode
{
    public string String()
    {
        return $"({Op.ToString()}({Value.String()}))";
    }
}

// ReSharper disable once InconsistentNaming
public enum UnaryOperation
{
    NOT, MINUS
}
