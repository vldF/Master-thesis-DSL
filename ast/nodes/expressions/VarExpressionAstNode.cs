using me.vldf.jsa.dsl.ast.nodes.declarations;

namespace me.vldf.jsa.dsl.ast.nodes.expressions;

public class VarExpressionAstNode(
    VarDeclAstNode variable) : ExpressionAstNode
{
    public VarDeclAstNode Variable { get; } = variable;

    public override string String()
    {
        return $"var({variable.Name})";
    }
}
