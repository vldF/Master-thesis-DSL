using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace me.vldf.jsa.dsl.ast.nodes.statements;

public class ReturnStatementAstNode(ExpressionAstNode? expression) : AstNode
{
    public override string String()
    {
        if (expression == null)
        {
            return "return";
        }

        return $"return {expression.String()}";
    }
}
