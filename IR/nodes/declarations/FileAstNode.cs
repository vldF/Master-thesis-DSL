using me.vldf.jsa.dsl.ast.nodes.statements;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FileAstNode(
    IReadOnlyCollection<IStatementAstNode> topLevelDeclarations) : IAstNode
{
    public IReadOnlyCollection<IStatementAstNode> TopLevelDeclarations { get; } = topLevelDeclarations;

    public string String()
    {
        var childrenAsString = string.Join("\n\n", TopLevelDeclarations.Select(x => x.String()));
        return $"""
                file (
                {AddIndent(childrenAsString)}
                )
                """;
    }
}
