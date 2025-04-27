using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using Codegen.IR.nodes.types;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace Semantics.Ast2CgIrTranslator.Emitters;

public class MethodEmitter(TranslatorContext ctx)
{
    private readonly ExpressionsEmitter _expressionsEmitter = new(ctx);

    public void Emit(FunctionAstNode func)
    {
        var locationArgName = "location";
        var locationArgType = new CgSimpleType("Location");

        var functionCallName = "functionCall";
        var functionCallType = new CgSimpleType("FunctionCall");

        var args = new Dictionary<string, ICgType>
        {
            { locationArgName, locationArgType },
            { functionCallName, functionCallType },
        };

        var returnType = new CgSimpleType("CallHandlerResult");

        ctx.HandlerMethod = ctx.File.CreateMethod(func.GetHandlerName(), args, returnType);
        ctx.PushContainer(ctx.HandlerMethod);

        var handledArgs = VarDeclaration(
            ctx.CurrentContainer!,
            "handledArguments",
            init: ctx.Semantics.ProcessorApi.CallMethod("ProcessKeywordArguments", [new CgVarExpression(functionCallName)]));
        foreach (var arg in func.Args)
        {
            var argVar = new CgVarExpression(arg.Name, isOutVar: true);
            var tryGetValueStatement = handledArgs.CallMethod(
                "TryGetValue",
                [AsExpression(arg.Name), argVar]);

            ctx.CurrentContainer!.Add(tryGetValueStatement);
        }

        EmitStatementBlockAstNode(func.Body);

        ctx.PopContainer();

        var argsAsList = func.Args.Select(a => AsExpression(a.Name)).ToList();
        var defaultArgs = func.Args
            .SkipWhile(a => a.DefaultValue == null)
            .Select(a =>
                a.DefaultValue != null
                    ? _expressionsEmitter.EmitExpression(a.DefaultValue)
                    : ctx.Semantics.SemanticsApi.Property("None"))
            .ToList();

        List<ICgExpression> argInfoArgs =
        [
            new CgListLiteralExpression(argsAsList),
            AsExpression(argsAsList.Count),
            AsExpression(false),
            AsExpression(false),
            AsExpression(0),
            AsExpression(0),
        ];

        if (defaultArgs.Count != 0)
        {
            var defaultsParam = new CgVarExpression("ImmutableSegment")
                .CallMethod("Create", defaultArgs, [new CgSimpleType("SymbolicExpression")]);
            argInfoArgs.Add(defaultsParam);
        }

        var argInfo = new CgNewExpression("ArgumentsInfo", argInfoArgs);
        var methodDescrName = func.GetMethodDescriptorName();
        var buildAndRegisterExpr = ctx.Semantics.ProcessorApi
            .CallMethod("CreateFunctionBuilder", [AsExpression(func.Name)])
            .CallMethod("WithHandler", [new CgVarExpression(func.GetHandlerName())])
            .CallMethod("WithArgumentsInfo", [argInfo]);

        if (func.Parent != null)
        {
            buildAndRegisterExpr = buildAndRegisterExpr.CallMethod("AssignTo", [ctx.CurrentClassDescriptor]);
        }
        else
        {
            buildAndRegisterExpr = buildAndRegisterExpr.CallMethod("AssignTo", [new CgVarExpression("ModuleDescriptor")]);
        }

        buildAndRegisterExpr = buildAndRegisterExpr.CallMethod("BuildAndRegister", []);


        ctx.File.AddVarDecl(methodDescrName, type: null, init: buildAndRegisterExpr);
    }

    private void EmitStatement(IStatementAstNode bodyChild)
    {
        switch (bodyChild)
        {
            case VarDeclAstNode varDeclAstNode:
                EmitVarDeclAstNode(varDeclAstNode);
                break;
            case IfStatementAstNode ifStatementAstNode:
                EmitIfStatement(ifStatementAstNode);
                break;
            case ReturnStatementAstNode returnStatementAstNode:
                EmitReturnStatement(returnStatementAstNode);
                break;
            case AssignmentAstNode varAssignmentAstNode:
                EmitVarAssignmentAstNode(varAssignmentAstNode);
                break;
            case StatementsBlockAstNode statementsBlockAstNode:
                EmitStatementBlockAstNode(statementsBlockAstNode);
                break;
            case FunctionCallAstNode qualifiedFunctionCallAstNode:
                EmitQualifiedFunctionCallAstNode(qualifiedFunctionCallAstNode);
                break;
            case IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode:
                EmitIntrinsicFunctionInvokationAstNode(intrinsicFunctionInvokationAstNode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(bodyChild));
        }
    }

    private void EmitIntrinsicFunctionInvokationAstNode(IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode)
    {
        ctx.CurrentContainer!.Add((CgMethodCall)_expressionsEmitter.EmitExpression(intrinsicFunctionInvokationAstNode));
    }

    private void EmitQualifiedFunctionCallAstNode(FunctionCallAstNode astCall)
    {
        var callStatement = (CgMethodCall)_expressionsEmitter.EmitExpression(astCall);
        ctx.CurrentContainer!.Add(callStatement);
    }

    private void EmitStatementBlockAstNode(StatementsBlockAstNode statementsBlockAstNode)
    {
        foreach (var bodyChild in statementsBlockAstNode.Children)
        {
            if (bodyChild is IStatementAstNode statement)
            {
                EmitStatement(statement);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(bodyChild));
            }
        }
    }

    private void EmitIfStatement(IfStatementAstNode ifStatementAstNode)
    {
        ctx.CurrentContainer!.Statements.Add(AsIfStatement(ifStatementAstNode));
    }

    private CgIfElseStatement AsIfStatement(IfStatementAstNode node)
    {
        var cond = _expressionsEmitter.EmitExpression(node.Cond);
        var ifStatement = new CgIfElseStatement(cond);

        ctx.PushContainer(ifStatement.MainBody);
        EmitStatementBlockAstNode(node.MainBlock);
        ctx.PopContainer();

        if (ifStatement.Elseif != null)
        {
            switch (node.ElseStatement)
            {
                case IfStatementAstNode elseIf:
                    ifStatement.Elseif = AsIfStatement(elseIf);
                    break;
                case StatementsBlockAstNode elseBlock:
                    ifStatement.ElseBody = new CgStatementsContainer();
                    ctx.PushContainer(ifStatement.ElseBody);
                    EmitStatementBlockAstNode(elseBlock);
                    ctx.PopContainer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(node.ElseStatement));
            }
        }

        return ifStatement;
    }

    private void EmitVarDeclAstNode(VarDeclAstNode varDeclAstNode)
    {
        if (varDeclAstNode.Init != null)
        {
            var initValue = _expressionsEmitter.EmitExpression(varDeclAstNode.Init);

            VarDeclaration(
                ctx.CurrentContainer!,
                varDeclAstNode.Name,
                type: null,
                init: initValue);
        }
        else
        {
            var symbolicType = new CgSimpleType("SymbolicExpression");
            VarDeclaration(
                ctx.CurrentContainer!,
                varDeclAstNode.Name,
                type: symbolicType,
                init: null);
        }
    }

    private void EmitVarAssignmentAstNode(AssignmentAstNode assignmentAstNode)
    {
        var reciever = assignmentAstNode.Reciever;
        if (reciever is VarExpressionAstNode varExpressionAstNode)
        {
            if (varExpressionAstNode.VariableReference.Resolve()!.IsLocalVarDecl)
            {
                EmitLocalVarAssignment(assignmentAstNode.Value, varExpressionAstNode);
                return;
            }
        }

        var (effectiveRecieverCg, fieldName) = _expressionsEmitter.EmitRecieverAndGet(reciever);

        var value = _expressionsEmitter.EmitExpression(assignmentAstNode.Value);
        var assignStatement = ctx.Semantics.InterpreterApi.CallMethod(
            "Assign",
            [effectiveRecieverCg.AsNoOutVar(), AsExpression(fieldName), value]);

        ctx.CurrentContainer!.Add(assignStatement);
    }

    private void EmitLocalVarAssignment(IExpressionAstNode expr, VarExpressionAstNode varExpressionAstNode)
    {
        var value = _expressionsEmitter.EmitExpression(expr);
        var varName = varExpressionAstNode.VariableReference.SealedValue!.Name;

        VarAssignment(
            ctx.CurrentContainer!,
            varName,
            value);
    }

    private void EmitReturnStatement(ReturnStatementAstNode statement)
    {
        ICgExpression? result;
        if (statement.Expression != null)
        {
            result = _expressionsEmitter.EmitExpression(statement.Expression);
        }
        else
        {
            result = null;
        }

        ctx.HandlerMethod.AddReturn(ctx.Semantics.Return(result));
    }
}
