using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.declarations;

public class FunctionAstNode(
    string name,
    IReadOnlyCollection<FunctionArgAstNode> args,
    TypeReference returnTypeRef,
    StatementsBlockAstNode body) : IStatementAstNode
{
    public string Name { get; } = name;

    public StatementsBlockAstNode Body { get; } = body;
    public List<FunctionArgAstNode> Args { get; } = args.ToList();
    public TypeReference? ReturnTypeRef { get; } = returnTypeRef;

    public string String()
    {
        var argsAsString = string.Join(", ", Args.Select(x => x.String()));
        var returnTypeAsStr = ReturnTypeRef != null ? ": @" + ReturnTypeRef.Id : "";
        return $"""
                func {Name} ({argsAsString}) {returnTypeAsStr} (
                {AddIndent(Body.String())}
                )
                """;
    }
}
