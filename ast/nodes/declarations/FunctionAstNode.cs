using me.vldf.jsa.dsl.ast.nodes.statements;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FunctionAstNode(
    string name,
    IReadOnlyCollection<FunctionArgAstNode> args,
    StatementsBlockAstNode body) : AstNode
{
    public string Name { get; } = name;

    public StatementsBlockAstNode Body { get; } = body;
    public IReadOnlyCollection<FunctionArgAstNode> Args { get; } = args;

    public override string String()
    {
        var argsAsString = string.Join(", ", Args.Select(x => x.String()));
        return $"""
                func {Name} ({argsAsString}) (
                {AddIndent(Body.String())}
                )
                """;
    }
}
