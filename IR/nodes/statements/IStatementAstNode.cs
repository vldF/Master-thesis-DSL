namespace me.vldf.jsa.dsl.ir.nodes.statements;

public interface IStatementAstNode : IAstNode
{
    IAstNode? Parent { get; set; }
}
