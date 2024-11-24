using Codegen.IR.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;

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
    public static CgBinExpression.BinOp ToCgBinOp(this BinaryOperation astBinOp)
    {
        Enum.TryParse(astBinOp.ToString(), out CgBinExpression.BinOp res);
        return res;
    }

    public static CgUnOp ToCgUnOp(this UnaryOperation astBinOp)
    {
        Enum.TryParse(astBinOp.ToString(), out CgUnOp res);
        return res;
    }
}
