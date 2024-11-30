using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public abstract class QualifiedAccessAstNodeBase(
    IExpressionAstNode parent
    ) : IExpressionAstNode, IStatementAstNode
{
    public IExpressionAstNode Parent { get; } = parent;
    public abstract string String();
}
