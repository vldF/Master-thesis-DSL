using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

// just a function call, not a semantic one
public record IntrinsicFunctionInvokationAstNode(
    IExpressionAstNode? Reciever,
    string Name,
    List<IExpressionAstNode> Args,
    List<TypeReference> Generics) : IExpressionAstNode, IStatementAstNode
{
    public string String()
    {
        var generics = string.Join(",", Generics.Select(x => x.AsString()));
        var args = string.Join(",", Args.Select(x => x.String()));
        return $"intrinsic[{Reciever?.String() ?? ""}.{Name}<{generics}>({args})]";
    }

    public IAstNode? Parent { get; set; } = null;
}
