using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class FileAstNode(
    string? package,
    IReadOnlyCollection<IStatementAstNode> topLevelDeclarations) : IAstNode
{
    public string? Package { get; } = package;

    public string? FileName { get; set; } = null;

    public IReadOnlyCollection<IStatementAstNode> TopLevelDeclarations { get; set; } = topLevelDeclarations;

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
