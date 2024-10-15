using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace me.vldf.jsa.dsl.ast.nodes.statements;

public class VarAssignmentAstNode(
    VarDeclAstNode variable,
    ExpressionAstNode value) : AstNode
{
    public override string String()
    {
        return $"{variable.Name} = {value.String()}";
    }
}
