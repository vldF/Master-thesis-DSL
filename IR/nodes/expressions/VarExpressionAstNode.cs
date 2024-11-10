using me.vldf.jsa.dsl.ast.nodes.declarations;

namespace me.vldf.jsa.dsl.ast.nodes.expressions;

public class VarExpressionAstNode(
    VarDeclAstNode variable) : IExpressionAstNode
{
    public VarDeclAstNode Variable { get; } = variable;

    public string String()
    {
        return $"var({Variable.Name})";
    }
}
