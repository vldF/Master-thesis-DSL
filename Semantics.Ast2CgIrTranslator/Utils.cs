using Codegen.IR.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator;

public static class Utils
{
    public static string GetHandlerName(this FunctionAstNodeBase node)
    {
        var objectNameOrNull = node.Parent as ObjectAstNode;
        return (objectNameOrNull?.Name ?? "global") + "_" + node.Name + "_" + "Handler";
    }

    public static string GetMethodDescriptorName(this FunctionAstNodeBase node)
    {
        var objectNameOrNull = node.Parent as ObjectAstNode;
        return (objectNameOrNull?.Name ?? "global") + "_" + node.Name + "_" + "MethodDescriptor";
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
