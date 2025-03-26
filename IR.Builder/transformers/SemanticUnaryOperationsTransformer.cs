using me.vldf.jsa.dsl.ir.helpers;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class SemanticUnaryOperationsTransformer : AbstractAstSemanticTransformer
{
    protected override IExpressionAstNode TransforUnaryAstNode(UnaryExpressionAstNode node)
    {
        node = (UnaryExpressionAstNode)base.TransforUnaryAstNode(node);

        var semanticEntityName = node.Op switch
        {
            UnaryOperation.NOT => "NotDescriptor",
            UnaryOperation.MINUS => "NegDescriptor",
            _ => throw new ArgumentOutOfRangeException()
        };

        var semanticEntityAccess = SemanticsApi.Property(semanticEntityName);

        return new IntrinsicFunctionInvokationAstNode(
            Interpretor,
            "InvokeFunction",
            [LocationArg, semanticEntityAccess, node.Value],
            []);
    }
}
