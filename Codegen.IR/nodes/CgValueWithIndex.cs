using Codegen.IR.nodes.expressions;

namespace Codegen.IR.nodes;

public record CgValueWithIndex(
    ICgExpression Reciever,
    int idx) : ICgStatement, ICgExpression;
