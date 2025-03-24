using me.vldf.jsa.dsl.ir.builder.transformers.python;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class TransformersOrchestrator(IrContext rootContext)
{
    private IrContext RootContext { get; } = rootContext;

    private readonly List<AbstractAstTransformer> _transformers =
    [
        new AddSelfArgumentTransformer(),
        new AddInitMethod(),
        new ReferenceSealer(),
        new FunctionCallTransformer(),
        new SemanticBinaryOperationsTransformer(),
        new SemanticUnaryOperationsTransformer(),
        new IfStatementsTransformer(),
        new ReturnTransformer(),
    ];

    public IAstNode Transform(IAstNode node)
    {
        var result = _transformers.Aggregate(
            node, (current, transformer) =>
            {
                transformer.Init(RootContext);
                return transformer.Transform(current);
            });

        return result;
    }
}
