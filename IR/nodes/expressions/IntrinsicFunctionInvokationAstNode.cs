namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public record IntrinsicFunctionInvokationAstNode(
    IExpressionAstNode Reciever,
    string Name,
    List<IExpressionAstNode> Args) : IExpressionAstNode
{
    public string String()
    {
        return $"intrinsic[{Reciever.String()}.{Name}({string.Join(",", Args)})[";
    }
}
