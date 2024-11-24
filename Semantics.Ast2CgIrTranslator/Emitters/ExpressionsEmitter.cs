using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Emitters;

public class ExpressionsEmitter(TranslatorContext ctx)
{
    public ICgExpression EmitExpression(IExpressionAstNode expression)
    {
        return expression switch
        {
            NewAstNode newAstNode => EmitNewNode(newAstNode),
            VarExpressionAstNode varExpressionAstNode => EmitVarExpressionNode(varExpressionAstNode),
            BinaryExpressionAstNode binaryExpressionAstNode => EmitBinaryExpressionNode(binaryExpressionAstNode),
            UnaryExpressionAstNode unaryExpressionAstNode => EmitUnaryExpressionNode(unaryExpressionAstNode),
            IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode =>
                EmitIntrinsicFunctionInvokationAstNode(intrinsicFunctionInvokationAstNode),
            QualifiedAccessAstNodeBase qualifiedAccessAstNode => EmitQualifiedAccessAstNodeBase(qualifiedAccessAstNode),

            _ => throw new ArgumentOutOfRangeException(nameof(expression))
        };
    }

    private ICgExpression EmitQualifiedAccessAstNodeBase(QualifiedAccessAstNodeBase qualifiedAccessAstNode)
    {
        return qualifiedAccessAstNode switch
        {
            QualifiedAccessPropertyAstNode node => EmitQualifiedAccessPropertyAstNode(node),
            _ => throw new ArgumentOutOfRangeException(nameof(qualifiedAccessAstNode))
        };
    }

    private ICgExpression EmitQualifiedAccessPropertyAstNode(QualifiedAccessPropertyAstNode node)
    {
        return new CgValueWithReciever(EmitExpression(node.Parent), node.PropertyName);
    }

    private ICgExpression EmitIntrinsicFunctionInvokationAstNode(IntrinsicFunctionInvokationAstNode functionInvoke)
    {
        return new CgMethodCall(
            EmitExpression(functionInvoke.Reciever),
            functionInvoke.Name,
            functionInvoke.Args.Select(EmitExpression).ToList());
    }

    private ICgExpression EmitBinaryExpressionNode(BinaryExpressionAstNode binaryExpressionAstNode)
    {
        return new CgBinExpression(
            EmitExpression(binaryExpressionAstNode.Left),
            EmitExpression(binaryExpressionAstNode.Right),
            binaryExpressionAstNode.Op.ToCgBinOp()
        );
    }

    private ICgExpression EmitUnaryExpressionNode(UnaryExpressionAstNode unaryExpressionAstNode)
    {
        return new CgUnaryExpression(
            EmitExpression(unaryExpressionAstNode.Value),
            unaryExpressionAstNode.Op.ToCgUnOp());
    }

    private ICgExpression EmitVarExpressionNode(VarExpressionAstNode varExpressionAstNode)
    {
        return new CgVarExpression(varExpressionAstNode.VariableReference.Resolve()!.Name);
    }

    private ICgExpression EmitNewNode(NewAstNode newAstNode)
    {
        var obj = newAstNode.TypeReference.SealedValue!;
        var descriptor = ctx.ClassDescriptorVariables[obj.Name];
        var args = newAstNode.Args.Select(EmitExpression);
        return ctx.Semantics.CreateInstance(obj.Name, descriptor, args);
    }
}
