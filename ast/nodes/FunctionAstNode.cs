namespace me.vldf.jsa.dsl.ast.nodes;

public class FunctionAstNode(string name) : AstNode
{
    public string Name { get; } = name;

    public override string String()
    {
        // var childrenAsString = string.Join("\n\n", TopLevelDeclarations.Select(x => x.String()));
        return $"""
                func {Name} (
                // todo
                )
                """;
    }
}
