namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FileAstNode(
    IReadOnlyCollection<AstNode> topLevelDeclarations) : AstNode
{
    public IReadOnlyCollection<AstNode> TopLevelDeclarations { get; } = topLevelDeclarations;

    public override string String()
    {
        var childrenAsString = string.Join("\n\n", TopLevelDeclarations.Select(x => x.String()));
        return $"""
                file (
                {AddIndent(childrenAsString)}
                )
                """;
    }
}
