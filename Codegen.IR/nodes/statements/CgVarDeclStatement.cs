using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes.statements;

public record CgVarDeclStatement(string Name, string? Type, ICgExpression? Init) : ICgStatement;
