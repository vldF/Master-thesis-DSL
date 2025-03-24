using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class AssignmentAstNode(
    IExpressionAstNode reciever,
    IExpressionAstNode value) : IStatementAstNode
{
    public IAstNode? Parent { get; set; } = null;

    public IExpressionAstNode Reciever { get; set; } = reciever;

    public IExpressionAstNode Value { get; set; } = value;

    public string String()
    {
        return $"@{Reciever.String()} = {Value.String()}";
    }
}
