using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace Semantics.Ast2CgIrTranslator.Emitters;

public class ExpressionsEmitter
{
    private readonly TranslatorContext ctx;
    private IntrinsicFunctionsCallWithNoImplEmitter _entrinsicFunctionsCallWithNoImplEmitter;

    public ExpressionsEmitter(TranslatorContext ctx)
    {
        this.ctx = ctx;
        _entrinsicFunctionsCallWithNoImplEmitter = new IntrinsicFunctionsCallWithNoImplEmitter(ctx, this);
    }

    public ICgExpression EmitExpression(IExpressionAstNode expression)
    {
        return expression switch
        {
            NewAstNode newAstNode => EmitNewNode(newAstNode),
            VarExpressionAstNode varExpressionAstNode => EmitVarExpressionNode(varExpressionAstNode),
            BinaryExpressionAstNode binaryExpressionAstNode => EmitBinaryExpressionNode(binaryExpressionAstNode),
            UnaryExpressionAstNode unaryExpressionAstNode => EmitUnaryExpressionNode(unaryExpressionAstNode),
            IntrinsicFunctionInvocationAstNode intrinsicFunctionInvokationAstNode =>
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
        var lit = new CgBoolLiteral(boolLiteralAstNode.Value);
        if (boolLiteralAstNode.IsSyntetic)
        {
            return lit;
        }

        return ctx.Semantics.SemanticsApi.CallMethod("CreateLiteral", [lit]);
    }

    private ICgExpression EmitFloatLiteralAstNode(FloatLiteralAstNode floatLiteralAstNode)
    {
        var lit = new CgFloatLiteral(floatLiteralAstNode.Value);
        if (floatLiteralAstNode.IsSyntetic)
        {
            return lit;
        }

        return ctx.Semantics.SemanticsApi.CallMethod("CreateLiteral", [lit]);
    }

    private ICgExpression EmitIntLiteralAstNode(IntLiteralAstNode intLiteralAstNode)
    {
        var lit = new CgIntLiteral(intLiteralAstNode.Value);
        if (intLiteralAstNode.IsSyntetic)
        {
            return lit;
        }

        return ctx.Semantics.SemanticsApi.CallMethod("CreateLiteral", [lit]);
    }

    private ICgExpression EmitStringLiteralAstNode(StringLiteralAstNode stringLiteralAstNode)
    {
        var lit = new CgStringLiteral(stringLiteralAstNode.Value);
        if (stringLiteralAstNode.IsSyntetic)
        {
            return lit;
        }

        return ctx.Semantics.SemanticsApi.CallMethod("CreateLiteral", [lit]);
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
        var resolvedFunc = node.FunctionReference.Resolve()!;

        var reciever = node.QualifiedParent != null
            ? EmitReciever(node.QualifiedParent)
            : new CgVarExpression("self");

        var locationArg = new CgVarExpression("location");

        var args = node.Args.Select(EmitExpression).ToList();

        var handlerName = resolvedFunc.GetMethodDescriptorName();
        var handlerVar = new CgVarExpression(handlerName);

        var invokeFuncArgs = new List<ICgExpression> { locationArg, handlerVar, reciever };
        invokeFuncArgs.AddRange(args);
        return ctx.Semantics.InterpreterApi.CallMethod("InvokeFunction", invokeFuncArgs);
    }

    private ICgExpression EmitQualifiedAccessPropertyAstNode(QualifiedAccessPropertyAstNode node)
    {
        if (node.IsSyntetic)
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

        if (varDeclAstNode.IsNone)
        {
            return ctx.Semantics.SemanticsApi.Property("None");
        }

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
        var obj = newAstNode.TypeReference.ResolveObject();
        if (obj == null)
        {
            throw new Exception($"unresolved {newAstNode.TypeReference.AsString()}");
        }
        var args = newAstNode.Args.Select(EmitExpression);
        var objDescrVar = new CgVarExpression(obj!.GetDescriptionVarName());

        return ctx.Semantics.CreateInstance(objDescrVar, args);
    }

    private ICgExpression EmitIntrinsicFunctionInvokationAstNode(IntrinsicFunctionInvocationAstNode intrinsicFunctionInvocationAstNode)
    {
        if (_entrinsicFunctionsCallWithNoImplEmitter.IsApplicable(intrinsicFunctionInvocationAstNode))
        {
            return _entrinsicFunctionsCallWithNoImplEmitter.Emit(intrinsicFunctionInvocationAstNode);
        }

        var qualifiedExpr = intrinsicFunctionInvocationAstNode.Reciever != null
            ? EmitExpression(intrinsicFunctionInvocationAstNode.Reciever)
            : null;

        var call = new CgMethodCall(
            qualifiedExpr,
            intrinsicFunctionInvocationAstNode.Name,
            intrinsicFunctionInvocationAstNode.Args.Select(EmitExpression).ToArray());

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
