using me.vldf.jsa.dsl.ast.nodes.statements;
using me.vldf.jsa.dsl.ast.types;

namespace me.vldf.jsa.dsl.ast.nodes.declarations;

public class FunctionAstNode(
    string name,
    IReadOnlyCollection<FunctionArgAstNode> args,
    AstType? returnType,
    StatementsBlockAstNode body) : IStatementAstNode
{
    public string Name { get; } = name;

    public StatementsBlockAstNode Body { get; } = body;
    public List<FunctionArgAstNode> Args { get; } = args.ToList();
    public AstType? ReturnType { get; } = returnType;

    public string String()
    {
        var argsAsString = string.Join(", ", Args.Select(x => x.String()));
        var returnTypeAsStr = ReturnType != null ? ": " + ReturnType.String() : "";
        return $"""
                func {Name} ({argsAsString}) {returnTypeAsStr} (
                {AddIndent(Body.String())}
                )
                """;
    }
}
