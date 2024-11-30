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
        ctx.CurrentBuilder = ctx.CurrentBuilder.CallMethod(
            "WithMethod",
            [AsExpression(func.Name), new CgVarExpression(func.GetHandlerName())]);

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

        foreach (var arg in func.Args)
        {
            EmitArg(arg);
        }

        EmitStatementBlockAstNode(func.Body);

        ctx.PopContainer();
    }

    private void EmitArg(FunctionArgAstNode argNode)
    {
        // todo: desugar it in preprocessing stages
        var initExpression = ctx.Semantics.GetArgument(argNode.Index);

        VarDeclaration(ctx.CurrentContainer!, argNode.Name, init: initExpression);
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
            case VarAssignmentAstNode varAssignmentAstNode:
                EmitVarAssignmentAstNode(varAssignmentAstNode);
                break;
            case StatementsBlockAstNode statementsBlockAstNode:
                EmitStatementBlockAstNode(statementsBlockAstNode);
                break;
            case QualifiedFunctionCallAstNode qualifiedFunctionCallAstNode:
                EmitQualifiedFunctionCallAstNode(qualifiedFunctionCallAstNode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(bodyChild));
        }
    }

    private void EmitQualifiedFunctionCallAstNode(QualifiedFunctionCallAstNode astCall)
    {
        var call = new CgFunctionCallWithReciever(
            _expressionsEmitter.EmitExpression(astCall.Parent),
            astCall.FunctionName,
            astCall.Args.Select(_expressionsEmitter.EmitExpression).ToArray());
        ctx.CurrentContainer!.Add(call);
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
            var initValue = _expressionsEmitter.EmitExpression(varDeclAstNode.Init!);
            VarDeclaration(
                ctx.CurrentContainer!,
                varDeclAstNode.Name,
                init: initValue);
        }
        else
        {
            var typeName = varDeclAstNode.TypeReference!.SealedValue!.Name;
            var type = new CgSimpleType(typeName);
            VarDeclaration(
                ctx.CurrentContainer!,
                varDeclAstNode.Name,
                type: type,
                init: null);
        }
    }

    private void EmitVarAssignmentAstNode(VarAssignmentAstNode varAssignmentAstNode)
    {
        var value = _expressionsEmitter.EmitExpression(varAssignmentAstNode.Value);
        var varName = varAssignmentAstNode.VariableReference.SealedValue!.Name;

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
