namespace me.vldf.jsa.dsl.ast.nodes.statements;

public class StatementsBlockAstNode(IReadOnlyCollection<IAstNode> children) : IStatementAstNode
{
    public IReadOnlyCollection<IAstNode> Children { get; } = children;

    public string String()
    {
        var content = string.Join("\n", Children.Select(x => x.String()));
        return $$"""
                 {
                 {{AddIndent(content)}}
                 }
                 """;
    }
}
