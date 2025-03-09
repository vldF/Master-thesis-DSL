using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class VarAssignmentAstNode(
    VariableReference variableReference,
    IExpressionAstNode value) : IStatementAstNode
{
    public IAstNode? Parent { get; set; } = null;

    public readonly VariableReference VariableReference = variableReference;
    public IExpressionAstNode Value { get; set; } = value;

    public string String()
    {
        return $"@{variableReference.AsString()} = {Value.String()}";
    }
}
