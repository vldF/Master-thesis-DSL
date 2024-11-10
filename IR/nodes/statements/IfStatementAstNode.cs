using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.nodes.statements;

public class IfStatementAstNode(
    IExpressionAstNode cond,
    StatementsBlockAstNode mainBlock,
    StatementsBlockAstNode? elseBlock) : IStatementAstNode
{
    public IExpressionAstNode Cond { get; set; } = cond;
    public StatementsBlockAstNode MainBlock { get; set; } = mainBlock;
    public StatementsBlockAstNode? ElseBlock { get; set; } = elseBlock;

    public string String()
    {
        var elsePart = ElseBlock != null ? "else " + ElseBlock.String() : string.Empty;
        return $"if ({Cond.String()}) {AddIndent(MainBlock.String())} {elsePart}";
    }
}
