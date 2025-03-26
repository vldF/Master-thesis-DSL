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
        var generics = string.Join(",", Generics);
        return $"intrinsic[{Reciever?.String() ?? ""}.{Name}<{generics}>({string.Join(",", Args)})]";
    }

    public IAstNode? Parent { get; set; } = null;
}
