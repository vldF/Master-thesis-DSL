using Codegen.IR.Builder;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;
using me.vldf.jsa.dsl.ast.nodes;
using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.statements;
using me.vldf.jsa.dsl.ast.visitors;
using static Codegen.IR.Builder.CodegenIrBuilder;

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

        foreach (var bodyChild in func.Body.Children)
        {
            if (bodyChild is IStatementAstNode statement)
            {
                EmitStatement(statement);
            }
        }
    }

    private void EmitStatement(IStatementAstNode bodyChild)
    {
        switch (bodyChild)
        {
            case VarDeclAstNode varDeclAstNode:
                break;
            case IfStatementAstNode ifStatementAstNode:
                break;
            case ReturnStatementAstNode returnStatementAstNode:
                EmitReturnStatement(returnStatementAstNode);
                break;
            case VarAssignmentAstNode varAssignmentAstNode:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(bodyChild));
        }
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

        ctx.HandlerMethod.AddReturn(ctx.SemanticTypes.Return(result));
    }
}
