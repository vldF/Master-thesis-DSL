using me.vldf.jsa.dsl.ast.nodes.statements;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class ObjectAstNode(
    string name,
    IReadOnlyCollection<IAstNode> children) : IStatementAstNode
{
    public string Name { get; } = name;

    public IReadOnlyCollection<IAstNode> Children { get; } = children ?? [];

    public string String()
    {
        var childrenAsString = string.Join("\n\n", Children.Select(x => x.String()));
        return $"""
                func {Name} (
                {AddIndent(childrenAsString)}
                )
                """;
    }
}
