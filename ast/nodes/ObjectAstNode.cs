namespace me.vldf.jsa.dsl.ast.nodes;

public class ObjectAstNode(
    string name,
    IReadOnlyCollection<AstNode> children) : AstNode
{
    public string Name { get; } = name;

    public IReadOnlyCollection<AstNode> Children { get; } = children;

    public override string String()
    {
        var childrenAsString = string.Join("\n\n", Children.Select(x => x.String()));
        return $"""
                func {Name} (
                {AddIndent(childrenAsString)}
                )
                """;
    }
}
