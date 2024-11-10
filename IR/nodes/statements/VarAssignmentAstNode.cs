using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace me.vldf.jsa.dsl.ast.nodes.statements;

public class VarAssignmentAstNode(
    VarDeclAstNode variable,
    IExpressionAstNode value) : IStatementAstNode
{
    public string String()
    {
        return $"{variable.Name} = {value.String()}";
    }
}
