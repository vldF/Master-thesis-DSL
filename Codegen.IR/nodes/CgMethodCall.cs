using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes;

public class CgMethodCall(
    ICgExpression Reciever,
    string Name,
    IReadOnlyCollection<ICgExpression> args) : ICgStatement, ICgExpression;
