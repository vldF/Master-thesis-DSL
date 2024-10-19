namespace Codegen.IR.nodes.expressions;

public record CgArrayIndexExpression(ICgExpression Reciever, CgIntLiteral Index) : ICgExpression;
