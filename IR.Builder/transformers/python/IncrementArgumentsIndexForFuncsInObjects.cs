using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;

namespace me.vldf.jsa.dsl.ir.builder.transformers.python;

/*
 * Because the first argument is `self`, we shift indexes by 1 when
 * a function is declared in an object
 */
public class IncrementArgumentsIndexForFuncsInObjects : AbstractAstTransformer
{
    protected override ObjectAstNode TransformObjectAstNode(ObjectAstNode node)
    {
        var result = new List<IAstNode>();

        foreach (var nodeChild in node.Children)
        {
            if (nodeChild is not FunctionAstNode func)
            {
                result.Add(nodeChild);
                continue;
            }

            func.Args = func.Args.Select(a =>
                {
                    a.Index++;
                    return a;
                })
                .ToList();

            result.Add(func);
        }

        node.Children = result;
        return node;
    }
}
