using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class VarExpressionAstNode(
    VariableReference variableReference,
    bool isOutVar = false) : IExpressionAstNode
{
    public VariableReference VariableReference { get; } = variableReference;
    public bool IsOutVar { get; set; } = isOutVar;

    public string String()
    {
        var outVarPrefix = IsOutVar ? "out var" : "";
        return $"var({outVarPrefix}@{VariableReference.AsString()})";
    }
}
