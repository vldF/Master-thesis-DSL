using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes;

public record CgFunctionCallWithReciever(
    ICgExpression Reciever,
    string Name,
    params ICgExpression[] Args) : ICgStatement, ICgExpression;
