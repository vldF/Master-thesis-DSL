using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

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
                object {Name} (
                {AddIndent(childrenAsString)}
                )
                """;
    }
}
