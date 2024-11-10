using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public record NewAstNode(
    TypeReference TypeReference,
    IReadOnlyCollection<IExpressionAstNode> Args) : IExpressionAstNode
{
    public string String()
    {
        var argsString = string.Join(", ", Args.Select(a => a.String()));
        return $"new @{TypeReference.AsString()}({argsString})";
    }
}
