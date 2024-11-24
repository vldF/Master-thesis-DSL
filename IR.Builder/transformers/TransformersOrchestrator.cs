using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class TransformersOrchestrator(IrContext rootContext)
{
    private IrContext RootContext { get; } = rootContext;

    private readonly List<AbstractAstTransformer> _transformers =
    [
        new ReferenceSealer(),
        new SemanticBinaryOperationsTransformer(),
        new SemanticUnaryOperationsTransformer(),
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
