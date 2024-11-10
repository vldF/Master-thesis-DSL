namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class StatementsBlockAstNode(IReadOnlyCollection<IAstNode> children) : IStatementAstNode
{
    public IReadOnlyCollection<IAstNode> Children { get; set; } = children;

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
