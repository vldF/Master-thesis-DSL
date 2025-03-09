using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

// just a function call, not a semantic one
public record IntrinsicFunctionInvokationAstNode(
    IExpressionAstNode? Reciever,
    string Name,
    List<IExpressionAstNode> Args) : IExpressionAstNode, IStatementAstNode
{
    public string String()
    {
        return $"intrinsic[{Reciever?.String() ?? ""}.{Name}({string.Join(",", Args)})]";
    }

    public IAstNode? Parent { get; set; } = null;
}
