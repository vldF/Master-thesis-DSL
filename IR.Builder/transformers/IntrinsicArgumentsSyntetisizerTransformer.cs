using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class IntrinsicArgumentsSyntetisizerTransformer : AbstractAstTransformer
{
    protected override IExpressionAstNode TransformFunctionCallAstNode(FunctionCallAstNode node)
    {
        var func = node.FunctionReference.Resolve()!;
        if (func is not IntrinsicFunctionAstNode)
        {
            return node;
        }

        foreach (var functionArgAstNode in node.Args)
        {
            if (functionArgAstNode is not IntLiteralAstNode
                && functionArgAstNode is not BoolLiteralAstNode
                && functionArgAstNode is not FloatLiteralAstNode
                && functionArgAstNode is not StringLiteralAstNode)
            {
                continue;
            }
            functionArgAstNode.IsSyntetic = true;
        }

        return node;
    }
}
