using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record ForeachStatement(string IteratorName, ICgExpression Iterable) : ICgStatement;
