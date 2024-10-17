using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record CgForeachStatement(string IteratorName, ICgExpression Iterable) : ICgStatement;
