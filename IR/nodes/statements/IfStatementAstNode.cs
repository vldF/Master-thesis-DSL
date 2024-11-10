using me.vldf.jsa.dsl.ast.nodes.expressions;

namespace me.vldf.jsa.dsl.ast.nodes.statements;

public class IfStatementAstNode(
    IExpressionAstNode cond,
    StatementsBlockAstNode mainBlock,
    IAstNode? elseNode) : IStatementAstNode
{
    public string String()
    {
        var elsePart = elseNode != null ? "else " + elseNode.String() : string.Empty;
        return $"if ({cond.String()}) {AddIndent(mainBlock.String())} {elsePart}";
    }
}
