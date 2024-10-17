using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record CgAssignmentStatement(ICgExpression Left, ICgExpression Right) : ICgStatement
{

}
