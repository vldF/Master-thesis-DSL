using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.helpers;

public static class ExpressionBuilder
{
    public static QualifiedAccessPropertyAstNode Property(this IExpressionAstNode parent, string name)
    {
        return new QualifiedAccessPropertyAstNode(parent, name);
    }

    public static IntrinsicFunctionInvokationAstNode Function(this VarExpressionAstNode parent, string name, params IExpressionAstNode[] args)
    {
        return new IntrinsicFunctionInvokationAstNode(parent, name, args.ToList());
    }
}
