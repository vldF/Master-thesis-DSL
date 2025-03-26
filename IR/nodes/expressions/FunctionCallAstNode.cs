using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.nodes.expressions;

public class FunctionCallAstNode(
    IExpressionAstNode? qualifiedParent,
    FunctionReference functionReference,
    IReadOnlyCollection<TypeReference> generics,
    params IExpressionAstNode[] args
    ) : QualifiedAccessAstNodeBase(qualifiedParent)
{
    public FunctionReference FunctionReference { get; } = functionReference;
    public IReadOnlyCollection<TypeReference> Generics { get; set; } = generics;
    public IExpressionAstNode[] Args { get; set; } = args;

    public override string String()
    {
        return $"({QualifiedParent?.String()}).{FunctionReference.AsString()}({string.Join<IExpressionAstNode>(", ", Args)})";
    }
}
