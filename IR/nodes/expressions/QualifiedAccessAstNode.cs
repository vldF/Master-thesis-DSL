using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public abstract class QualifiedAccessAstNodeBase(
    IExpressionAstNode? qualifiedParent
    ) : IExpressionAstNode, IStatementAstNode
{
    public IExpressionAstNode? QualifiedParent { get; } = qualifiedParent;
    public abstract string String();
}
