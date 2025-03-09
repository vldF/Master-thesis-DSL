using me.vldf.jsa.dsl.ir.context;

namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class StatementsBlockAstNode(IReadOnlyCollection<IAstNode> children) : IStatementAstNode, IContextOwner
{
    public IrContext Context { get; set; }

    public IAstNode? Parent { get; set; } = null;

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
