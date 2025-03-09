using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class ThisTransformer : AbstractAstSemanticTransformer
{
    protected override IExpressionAstNode TransformExpressionAstNode(IExpressionAstNode node)
    {
        return node;
    }
}
