using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class ObjectAstNode(
    string name,
    IReadOnlyCollection<IAstNode> children,
    IrContext context) : IStatementAstNode, IContextOwner
{
    public IrContext Context { get; set; } = context;

    public IAstNode? Parent { get; set; } = null;

    public string Name { get; } = name;

    public IReadOnlyCollection<IAstNode> Children { get; set; } = children ?? [];

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
