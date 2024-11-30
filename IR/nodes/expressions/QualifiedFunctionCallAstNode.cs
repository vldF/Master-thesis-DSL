namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class QualifiedFunctionCallAstNode(
    IExpressionAstNode parent,
    string functionName,
    params IExpressionAstNode[] args
    ) : QualifiedAccessAstNodeBase(parent)
{
    public string FunctionName { get; } = functionName;
    public IExpressionAstNode[] Args { get; } = args;

    public override string String()
    {
        return $"({Parent.String()}).{FunctionName}({string.Join<IExpressionAstNode>(", ", Args)})";
    }
}
