namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public abstract class QualifiedAccessAstNodeBase(
    IExpressionAstNode parent
    ) : IExpressionAstNode
{
    public IExpressionAstNode Parent { get; } = parent;
    public abstract string String();
}
