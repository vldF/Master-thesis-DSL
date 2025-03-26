using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class FunctionCallTransformer : AbstractAstSemanticTransformer
{
    protected override IExpressionAstNode TransformFunctionCallAstNode(FunctionCallAstNode node)
    {
        var args = node.Args.ToList();
        args.Insert(0, LocationArg);
        node.Args = args.ToArray();

        return node.FunctionReference.Resolve() is IntrinsicFunctionAstNode
            ? TransformIntrinsicFunctionCall(node)
            : TransformFunctionCall(node);
    }

    private IExpressionAstNode TransformIntrinsicFunctionCall(FunctionCallAstNode node)
    {
        var func = node.FunctionReference.Resolve()!;
        return new IntrinsicFunctionInvokationAstNode(null, func.Name, node.Args.ToList(), node.Generics.ToList());
    }

    private IExpressionAstNode TransformFunctionCall(FunctionCallAstNode node)
    {
        throw new NotImplementedException();
    }
}
