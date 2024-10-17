using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record CgForStatement(string varName, ICgStatement iterExpr, ICgExpression cond) : ICgStatement;
