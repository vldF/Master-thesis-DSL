using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes;

public record CgMethodCall(
    ICgExpression Reciever,
    string Name,
    IReadOnlyCollection<ICgExpression> Args) : ICgStatement, ICgExpression;
