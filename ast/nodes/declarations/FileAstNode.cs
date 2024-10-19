namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FileAstNode(
    IReadOnlyCollection<IAstNode> topLevelDeclarations) : IAstNode
{
    public IReadOnlyCollection<IAstNode> TopLevelDeclarations { get; } = topLevelDeclarations;

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
