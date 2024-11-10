using me.vldf.jsa.dsl.ir.nodes.declarations;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class VarExpressionAstNode(
    VarDeclAstNode variable) : IExpressionAstNode
{
    public VarDeclAstNode Variable { get; } = variable;

    public string String()
    {
        return $"var({Variable.Name})";
    }
}
