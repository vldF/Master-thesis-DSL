using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record CgReturnStatement(ICgExpression? Value = null) : ICgStatement;
