using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace me.vldf.jsa.dsl.ast.nodes.statements;

public class ReturnStatementAstNode(IExpressionAstNode? expression) : IStatementAstNode
{
    public readonly IExpressionAstNode? Expression = expression;

    public string String()
    {
        if (Expression == null)
        {
            return "return";
        }

        return $"return {Expression.String()}";
    }
}
