using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class VarAssignmentAstNode(
    VarDeclAstNode variable,
    IExpressionAstNode value) : IStatementAstNode
{
    public string String()
    {
        return $"{variable.Name} = {value.String()}";
    }
}
