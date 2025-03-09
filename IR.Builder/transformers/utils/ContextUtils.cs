using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.builder.transformers.utils;

public static class ContextUtils
{
    public static IrContext? GetNearestContext(this IAstNode node)
    {
        while (true)
        {
            if (node is IContextOwner contextOwner)
            {
                return contextOwner.Context;
            }

            if (node is not IStatementAstNode { Parent: not null } statementAstNode)
            {
                return null;
            }

            node = statementAstNode.Parent;
        }
    }
}
