using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public record NewAstNode(
    TypeReference typeReference,
    IReadOnlyCollection<IExpressionAstNode> args) : IExpressionAstNode
{
    public string String()
    {
        var argsString = string.Join(", ", args.Select(a => a.String()));
        return $"new @{typeReference.Id}({argsString})";
    }
}
