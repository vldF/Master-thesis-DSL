using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class FileAstNode(
    string? package,
    IReadOnlyCollection<IStatementAstNode> topLevelDeclarations,
    IrContext context) : IAstNode, IContextOwner
{
    public string? Package { get; } = package;

    public string? FileName { get; set; } = null;

    public IReadOnlyCollection<IStatementAstNode> TopLevelDeclarations { get; set; } = topLevelDeclarations;

    public IrContext Context { get; set; } = context;

    public string String()
    {
        var childrenAsString = string.Join("\n\n", TopLevelDeclarations.Select(x => x.String()));
        return $"""
                file (
                package: {Package}
                {AddIndent(childrenAsString)}
                )
                """;
    }
}
