using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class IfStatementAstNode(
    IExpressionAstNode cond,
    StatementsBlockAstNode mainBlock,
    IStatementAstNode? elseStatement) : IStatementAstNode
{
    public IExpressionAstNode Cond { get; set; } = cond;
    public StatementsBlockAstNode MainBlock { get; set; } = mainBlock;
    public IStatementAstNode? ElseStatement { get; set; } = elseStatement;

    public string String()
    {
        var elsePart = ElseStatement != null ? "else " + ElseStatement.String() : string.Empty;
        return $"if ({Cond.String()}) {AddIndent(MainBlock.String())} {elsePart}";
    }
}
