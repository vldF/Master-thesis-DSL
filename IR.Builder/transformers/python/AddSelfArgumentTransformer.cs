using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.builder.transformers.python;

public class AddSelfArgumentTransformer : AbstractAstSemanticTransformer
{
    protected override FunctionAstNode TransformFunctionAstNode(FunctionAstNode node)
    {
        if (node.Parent is not ObjectAstNode parent)
        {
            return node;
        }

        var selfArgType = new TypeReference(parent.Name, node.Context);
        var selfArg = new FunctionArgAstNode("self", selfArgType, 0);
        node.Args.Insert(0, selfArg);

        foreach (var functionArgAstNode in node.Args[1..])
        {
            functionArgAstNode.Index++;
        }

        return node;
    }
}
