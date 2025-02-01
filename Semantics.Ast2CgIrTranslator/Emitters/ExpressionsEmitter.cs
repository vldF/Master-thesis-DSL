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
            IntLiteralAstNode intLiteralAstNode => EmitIntLiteralAstNode(intLiteralAstNode),
            FloatLiteralAstNode floatLiteralAstNode => EmitFloatLiteralAstNode(floatLiteralAstNode),
            BoolLiteralAstNode boolLiteralAstNode => EmitBoolLiteralAstNode(boolLiteralAstNode),
            StringLiteralAstNode stringLiteralAstNode => EmitStringLiteralAstNode(stringLiteralAstNode),

            _ => throw new ArgumentOutOfRangeException(nameof(expression))
        };
    }

    private ICgExpression EmitBoolLiteralAstNode(BoolLiteralAstNode boolLiteralAstNode)
    {
        return new CgBoolLiteral(boolLiteralAstNode.Value);
    }

    private ICgExpression EmitFloatLiteralAstNode(FloatLiteralAstNode floatLiteralAstNode)
    {
        return new CgFloatLiteral(floatLiteralAstNode.Value);
    }

    private ICgExpression EmitIntLiteralAstNode(IntLiteralAstNode intLiteralAstNode)
    {
        return new CgIntLiteral(intLiteralAstNode.Value);
    }

    private ICgExpression EmitStringLiteralAstNode(StringLiteralAstNode stringLiteralAstNode)
    {
        return new CgStringLiteral(stringLiteralAstNode.Value);
    }

    private ICgExpression EmitQualifiedAccessAstNodeBase(QualifiedAccessAstNodeBase qualifiedAccessAstNode)
    {
        return qualifiedAccessAstNode switch
        {
            QualifiedAccessPropertyAstNode node => EmitQualifiedAccessPropertyAstNode(node),
            FunctionCallAstNode node => EmitQualifiedFunctionCallAstNode(node),
            _ => throw new ArgumentOutOfRangeException(nameof(qualifiedAccessAstNode))
        };
    }

    private ICgExpression EmitQualifiedFunctionCallAstNode(FunctionCallAstNode node)
    {
        var qualifiedExpr = node.QualifiedParent != null ? EmitExpression(node.QualifiedParent) : null;
        return new CgMethodCall(
            qualifiedExpr,
            node.FunctionReference.SealedValue!.Name,
            node.Args.Select(EmitExpression).ToArray());
    }

    private ICgExpression EmitQualifiedAccessPropertyAstNode(QualifiedAccessPropertyAstNode node)
    {
        return new CgValueWithReciever(EmitExpression(node.QualifiedParent!), node.PropertyName);
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
        return new CgVarExpression(
            varExpressionAstNode.VariableReference.Resolve()!.Name,
            isOutVar: varExpressionAstNode.IsOutVar);
    }

    private ICgExpression EmitNewNode(NewAstNode newAstNode)
    {
        var obj = newAstNode.TypeReference.SealedValue!;
        var descriptor = ctx.ClassDescriptorVariables[obj.Name];
        var args = newAstNode.Args.Select(EmitExpression);
        return ctx.Semantics.CreateInstance(obj.Name, descriptor, args);
    }

    private CgMethodCall EmitIntrinsicFunctionInvokationAstNode(IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode)
    {
        var qualifiedExpr = intrinsicFunctionInvokationAstNode.Reciever != null
            ? EmitExpression(intrinsicFunctionInvokationAstNode.Reciever)
            : null;

        var call = new CgMethodCall(
            qualifiedExpr,
            intrinsicFunctionInvokationAstNode.Name,
            intrinsicFunctionInvokationAstNode.Args.Select(EmitExpression).ToArray());

        return call;
    }
}
