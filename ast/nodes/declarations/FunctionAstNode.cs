using me.vldf.jsa.dsl.ast.nodes.statements;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FunctionAstNode(
    string name, StatementsBlockAstNode body) : AstNode
{
    public string Name { get; } = name;

    public StatementsBlockAstNode Body { get; } = body;

    public override string String()
    {
        return $"""
                func {Name} (
                {AddIndent(Body.String())}
                )
                """;
    }
}
