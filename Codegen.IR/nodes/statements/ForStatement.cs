using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record ForStatement(string varName, ICgStatement iterExpr, ICgExpression cond) : ICgStatement;
