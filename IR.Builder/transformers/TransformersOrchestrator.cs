using me.vldf.jsa.dsl.ir.nodes;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class TransformersOrchestrator
{
    private readonly List<AbstractAstTransformer> _transformers =
    [
        new ReferenceSealer(),
    ];

    public IAstNode Transform(IAstNode node)
    {
        var result = _transformers.Aggregate(
            node, (current, transformer) => transformer.Transform(current));

        return result;
    }
}
