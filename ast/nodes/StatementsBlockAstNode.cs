namespace me.vldf.jsa.dsl.ast.nodes;

public class StatementsBlockAstNode(IReadOnlyCollection<AstNode> children) : AstNode
{
    public IReadOnlyCollection<AstNode> Children { get; } = children;

    public override string String()
    {
        var content = string.Join("\n\n", Children.Select(x => x.String()));
        return $$"""
                 {
                 {{AddIndent(content)}}
                 }
                 """;
    }
}
