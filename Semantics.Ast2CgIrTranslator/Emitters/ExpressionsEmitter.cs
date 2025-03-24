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
        if (node.IsSyntatic)
        {
            return new CgValueWithReciever(EmitExpression(node.QualifiedParent!), node.PropertyName);
        }

        var (effectiveReciever, propName) = EmitRecieverAndGet(node);

        var result = new CgVarExpression(ctx.GetFreshVarName("tryGetResult"), isOutVar: true);
        var assignStatement = ctx.Semantics.InterpreterApi.CallMethod(
            "TryGetValue",
            [effectiveReciever.AsNoOutVar(), AsExpression(propName), result]);

        ctx.CurrentContainer!.Add(assignStatement);

        return result.AsNoOutVar();
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
        var varDeclAstNode = varExpressionAstNode.VariableReference.Resolve()!;
        if (varDeclAstNode.IsLocalVarDecl)
        {
            return new CgVarExpression(
                varDeclAstNode!.Name,
                isOutVar: varExpressionAstNode.IsOutVar);
        }

        var (effectiveReciever, propName) = EmitRecieverAndGet(varExpressionAstNode);

        var result = new CgVarExpression(ctx.GetFreshVarName("tryGetResult"), isOutVar: true);
        var assignStatement = ctx.Semantics.InterpreterApi.CallMethod(
            "TryGetValue",
            [effectiveReciever.AsNoOutVar(), AsExpression(propName), result]);

        ctx.CurrentContainer!.Add(assignStatement);

        return result.AsNoOutVar();
    }

    private ICgExpression EmitNewNode(NewAstNode newAstNode)
    {
        var obj = newAstNode.TypeReference.SealedValue!;
        var args = newAstNode.Args.Select(EmitExpression);
        return ctx.Semantics.CreateInstance(obj.Name, args);
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

    public (CgVarExpression effectiveRecieverCg, string fieldName) EmitRecieverAndGet(IExpressionAstNode reciever)
    {
        CgVarExpression effectiveRecieverCg;
        string fieldName;
        switch (reciever)
        {
            case VarExpressionAstNode varReciever:
                // [self].property case
                effectiveRecieverCg = new CgVarExpression("self");
                fieldName = varReciever.VariableReference.Resolve()!.Name;
                break;
            case QualifiedAccessPropertyAstNode qualReciever:
                // reciever.field case
                effectiveRecieverCg = EmitReciever(qualReciever.QualifiedParent!);
                fieldName = qualReciever.PropertyName;
                break;
            default:
                throw new InvalidOperationException("unimplemented");
        }

        return (effectiveRecieverCg, fieldName);
    }

    private CgVarExpression EmitReciever(IExpressionAstNode expr)
    {
        switch (expr)
        {
            case VarExpressionAstNode varExpr:
            {
                var reference = varExpr.VariableReference;
                var varDecl = reference.Resolve()!;
                if (!varDecl.IsObjectField())
                {
                    return new CgVarExpression(varDecl.Name);
                }

                var selfVar = new CgVarExpression("self");
                var tryGetValueResult = new CgVarExpression(ctx.GetFreshVarName("tryGetValueResult"), isOutVar: true);
                var tryGetValueCall = ctx.Semantics.InterpreterApi.CallMethod(
                    "TryGetValue",
                    [selfVar, AsExpression(varDecl.Name), tryGetValueResult]);

                ctx.CurrentContainer!.Add(tryGetValueCall);
                return tryGetValueResult;
            }

            case QualifiedAccessPropertyAstNode qualExpr:
            {
                var parent = EmitReciever(qualExpr.QualifiedParent!);

                var tryGetValueResult = new CgVarExpression(ctx.GetFreshVarName("tryGetValueResult"), isOutVar: true);
                var tryGetValueCall = ctx.Semantics.InterpreterApi.CallMethod(
                    "TryGetValue",
                    [parent, AsExpression(qualExpr.PropertyName), tryGetValueResult]);

                ctx.CurrentContainer!.Add(tryGetValueCall);

                return tryGetValueResult;
            }

            default:
                throw new InvalidOperationException("unimplemented");
        }
    }
}
