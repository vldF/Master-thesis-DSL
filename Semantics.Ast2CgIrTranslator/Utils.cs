using me.vldf.jsa.dsl.ast.nodes.declarations;

namespace Semantics.Ast2CgIrTranslator;

public static class Utils
{
    public static string GetHandlerName(this FunctionAstNode node)
    {
        return node.Name + "Handler";
    }

    public static string GetDescriptionVarName(this ObjectAstNode node)
    {
        return node.Name + "ClassDescription";
    }
}
