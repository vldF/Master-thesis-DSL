using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class ObjectAstNode(
    string name,
    IReadOnlyCollection<IAstNode> children,
    ICollection<AnnotationAstNode> annotations,
    IrContext context) : IStatementAstNode, IContextOwner
{
    public ICollection<AnnotationAstNode> Annotations { get; } = annotations;
    public IrContext Context { get; set; } = context;

    public IAstNode? Parent { get; set; } = null;

    public string Name { get; } = name;

    public IReadOnlyCollection<IAstNode> Children { get; set; } = children ?? [];

    public string String()
    {
        var childrenAsString = string.Join("\n\n", Children.Select(x => x.String()));
        var annos = string.Join(", ", Annotations.Select(a => a.String()));
        return $"""
                {annos}
                object {Name} (
                {AddIndent(childrenAsString)}
                )
                """;
    }
}
