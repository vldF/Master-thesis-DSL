using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.builder.transformers.utils;

public class NodeFinder<T> : AbstractAstTransformer
{
    private readonly List<T> _results = [];

    public IReadOnlyCollection<T> FindAllNodesOfType(IAstNode node)
    {
        Transform(node);
        return _results;
    }

    public T? FindFirstNodeOfType(IAstNode node)
    {
        Transform(node);
        return _results.FirstOrDefault();
    }

    protected override IExpressionAstNode TransformExpressionAstNode(IExpressionAstNode node)
    {
        if (node is T castedNode)
        {
            _results.Add(castedNode);
        }

        return base.TransformExpressionAstNode(node);
    }

    protected override IStatementAstNode TransformStatementAstNode(IStatementAstNode node)
    {
        if (node is T castedNode)
        {
            _results.Add(castedNode);
        }

        return base.TransformStatementAstNode(node);
    }
}
