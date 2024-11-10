using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class VarExpressionAstNode(
    VariableReference variableReference) : IExpressionAstNode
{
    public VariableReference VariableReference { get; } = variableReference;

    public string String()
    {
        return $"var(@{VariableReference.AsString()})";
    }
}
