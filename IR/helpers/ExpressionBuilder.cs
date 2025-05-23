using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.helpers;

public static class ExpressionBuilder
{
    public static QualifiedAccessPropertyAstNode Property(this IExpressionAstNode parent, string name)
    {
        return new QualifiedAccessPropertyAstNode(parent, name)
        {
            Parent = parent,
            IsSyntetic = true,
        };
    }

    public static IntrinsicFunctionInvocationAstNode Function(this VarExpressionAstNode parent, string name, params IExpressionAstNode[] args)
    {
        return new IntrinsicFunctionInvocationAstNode(parent, name, args.ToList(), [])
        {
            Parent = parent
        };
    }
}
