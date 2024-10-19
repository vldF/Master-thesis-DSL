using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record CgIfElseStatement(ICgExpression Cond) : ICgStatement
{
    public CgStatementsContainer MainBody = new();
    public CgStatementsContainer ElseBody = new();
}
