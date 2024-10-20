using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes;

public record CgValueWithReciever(
    ICgExpression Reciever,
    string Name) : ICgStatement, ICgExpression;
