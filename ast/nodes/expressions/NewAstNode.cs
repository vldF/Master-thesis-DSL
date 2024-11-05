namespace me.vldf.jsa.dsl.ast.nodes.expressions;

public record NewAstNode(
    string objectName,
    IReadOnlyCollection<IExpressionAstNode> args) : IExpressionAstNode
{
    public string String()
    {
        var argsString = string.Join(", ", args.Select(a => a.String()));
        return $"new {objectName}({argsString})";
    }
}
