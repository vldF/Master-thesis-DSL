namespace Codegen.IR.nodes.expressions;

public record CgArrayIndexExpression(ICgExpression Reciever, CgIntExpression Index) : ICgExpression;
