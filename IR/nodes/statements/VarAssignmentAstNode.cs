using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class VarAssignmentAstNode(
    VariableReference variableReference,
    IExpressionAstNode value) : IStatementAstNode
{
    public readonly VariableReference VariableReference = variableReference;
    public IExpressionAstNode Value { get; set; } = value;

    public string String()
    {
        return $"@{variableReference.AsString()} = {Value.String()}";
    }
}
