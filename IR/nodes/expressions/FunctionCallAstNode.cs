using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class FunctionCallAstNode(
    IExpressionAstNode? qualifiedParent,
    FunctionReference functionReference,
    params IExpressionAstNode[] args
    ) : QualifiedAccessAstNodeBase(qualifiedParent)
{
    public FunctionReference FunctionReference { get; } = functionReference;
    public IExpressionAstNode[] Args { get; set; } = args;

    public override string String()
    {
        return $"({QualifiedParent?.String()}).{FunctionReference.AsString()}({string.Join<IExpressionAstNode>(", ", Args)})";
    }
}
