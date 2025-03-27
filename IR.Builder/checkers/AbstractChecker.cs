using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.builder.checkers;

public abstract class AbstractCheckerBase
{
    public abstract void Check(FileAstNode file);
}

public abstract class AbstractChecker<TR> : AbstractCheckerBase
{
    protected abstract TR? Merge(TR? first, TR? second);

    public override void Check(FileAstNode file)
    {
        CheckFile(file);
    }

    protected virtual TR? CheckFile(FileAstNode file)
    {
        foreach (var decl in file.TopLevelDeclarations)
        {
            CheckStatement(decl);
        }

        return default;
    }

    protected virtual TR? CheckObject(ObjectAstNode obj)
    {
        foreach (var objChild in obj.Children)
        {
            CheckAstNode(objChild);
        }

        return default;
    }

    protected virtual TR? CheckAstNode(IAstNode node)
    {
        switch (node)
        {
            case FileAstNode fileAstNode:
                return CheckFile(fileAstNode);
            case FunctionArgAstNode functionArgAstNode:
                return CheckFunctionArg(functionArgAstNode);
            case FunctionAstNode functionAstNode:
                return CheckFunction(functionAstNode);
            case IntrinsicFunctionAstNode intrinsicFunctionAstNode:
                return CheckIntrinsicFunction(intrinsicFunctionAstNode);
            case FunctionAstNodeBase functionAstNodeBase:
                return CheckFunctionBase(functionAstNodeBase);
            case ObjectAstNode objectAstNode:
                return CheckObject(objectAstNode);
            case VarDeclAstNode varDeclAstNode:
                return CheckVarDecl(varDeclAstNode);
            case FunctionCallAstNode functionCallAstNode:
                return CheckFunctionCall(functionCallAstNode);
            case QualifiedAccessAstNodeBase qualifiedAccessAstNodeBase:
                return CheckQualifiedAccessBase(qualifiedAccessAstNodeBase);
            case IExpressionAstNode expressionAstNode:
                return CheckExpression(expressionAstNode);
            case AssignmentAstNode assignmentAstNode:
                return CheckAssignment(assignmentAstNode);
            case IfStatementAstNode ifStatementAstNode:
                return CheckIf(ifStatementAstNode);
            case ReturnStatementAstNode returnStatementAstNode:
                return CheckReturn(returnStatementAstNode);
            case StatementsBlockAstNode statementsBlockAstNode:
                return CheckStatementsBlock(statementsBlockAstNode);
            case IStatementAstNode statementAstNode:
                return CheckStatement(statementAstNode);
            default:
                throw new ArgumentOutOfRangeException(nameof(node));
        }
    }

    protected virtual TR? CheckStatementsBlock(StatementsBlockAstNode statementsBlockAstNode)
    {
        foreach (var objChild in statementsBlockAstNode.Children)
        {
            CheckAstNode(objChild);
        }

        return default;
    }

    protected virtual TR? CheckReturn(ReturnStatementAstNode returnStatementAstNode)
    {
        return returnStatementAstNode.Expression != null
            ? CheckExpression(returnStatementAstNode.Expression)
            : default;
    }

    protected virtual TR? CheckFunctionBase(FunctionAstNodeBase functionAstNodeBase)
    {
        return functionAstNodeBase switch
        {
            FunctionAstNode functionAstNode => CheckFunction(functionAstNode),
            IntrinsicFunctionAstNode intrinsicFunctionAstNode => CheckIntrinsicFunction(intrinsicFunctionAstNode),
            _ => throw new ArgumentOutOfRangeException(nameof(functionAstNodeBase))
        };
    }

    protected virtual TR? CheckIf(IfStatementAstNode ifStatementAstNode)
    {
        TR? result = default;
        if (ifStatementAstNode.ElseStatement != null)
        {
            result = CheckStatement(ifStatementAstNode.ElseStatement);
        }

        result = Merge(result, CheckExpression(ifStatementAstNode.Cond));
        result = Merge(result, CheckStatementsBlock(ifStatementAstNode.MainBlock));

        return result;
    }

    protected virtual TR? CheckAssignment(AssignmentAstNode assignmentAstNode)
    {
        return CheckExpression(assignmentAstNode.Value);
    }

    protected virtual TR? CheckExpression(IExpressionAstNode expressionAstNode)
    {
        switch (expressionAstNode)
        {
            case BinaryExpressionAstNode binaryExpressionAstNode:
                return Merge(
                    CheckExpression(binaryExpressionAstNode.Left),
                    CheckExpression(binaryExpressionAstNode.Right)
                );
            case FunctionCallAstNode functionCallAstNode:
                return CheckFunctionCall(functionCallAstNode);
            case UnaryExpressionAstNode unaryExpressionAstNode:
                return CheckExpression(unaryExpressionAstNode.Value);
            case QualifiedAccessAstNodeBase qualifiedAccessAstNodeBase:
                return CheckQualifiedAccessBase(qualifiedAccessAstNodeBase);
            case IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode:
                return CheckIntrinsicFunctionInvocation(intrinsicFunctionInvokationAstNode);
            case NewAstNode newAstNode:
                return CheckNew(newAstNode);
            case BoolLiteralAstNode:
                break;
            case FloatLiteralAstNode:
                break;
            case IntLiteralAstNode:
                break;
            case StringLiteralAstNode:
                break;
            case VarExpressionAstNode:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(expressionAstNode));
        }

        return default;
    }

    protected virtual TR? CheckNew(NewAstNode newAstNode)
    {
        foreach (var arg in newAstNode.Args)
        {
            CheckExpression(arg);
        }

        return default;
    }

    protected virtual TR? CheckIntrinsicFunctionInvocation(IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode)
    {
        foreach (var arg in intrinsicFunctionInvokationAstNode.Args)
        {
            CheckExpression(arg);
        }

        return default;
    }

    protected abstract TR? CheckQualifiedAccessBase(QualifiedAccessAstNodeBase qualifiedAccessAstNodeBase);
    protected abstract TR? CheckFunctionCall(FunctionCallAstNode functionCallAstNode);
    protected abstract TR? CheckVarDecl(VarDeclAstNode varDeclAstNode);
    protected abstract TR? CheckIntrinsicFunction(IntrinsicFunctionAstNode intrinsicFunctionAstNode);
    protected abstract TR? CheckFunctionArg(FunctionArgAstNode functionArgAstNode);

    protected virtual TR? CheckStatement(IStatementAstNode statement)
    {
        switch (statement)
        {
            case FunctionAstNodeBase functionAstNodeBase:
                return CheckFunctionBase(functionAstNodeBase);
            case FunctionArgAstNode functionArgAstNode:
                return CheckFunctionArg(functionArgAstNode);
            case ObjectAstNode objectAstNode:
                return CheckObject(objectAstNode);
            case VarDeclAstNode varDeclAstNode:
                return CheckVarDecl(varDeclAstNode);
            case FunctionCallAstNode functionCallAstNode:
                return CheckFunctionCall(functionCallAstNode);
            case IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode:
                return CheckIntrinsicFunctionInvokation(intrinsicFunctionInvokationAstNode);
            case QualifiedAccessAstNodeBase qualifiedAccessAstNodeBase:
                return CheckQualifiedAccessBase(qualifiedAccessAstNodeBase);
            case AssignmentAstNode assignmentAstNode:
                return CheckAssignment(assignmentAstNode);
            case IfStatementAstNode ifStatementAstNode:
                return CheckIf(ifStatementAstNode);
            case ReturnStatementAstNode returnStatementAstNode:
                return CheckReturn(returnStatementAstNode);
            case StatementsBlockAstNode statementsBlockAstNode:
                return CheckStatementsBlock(statementsBlockAstNode);
            default:
                throw new ArgumentOutOfRangeException(nameof(statement));
        }
    }

    protected virtual TR? CheckIntrinsicFunctionInvokation(IntrinsicFunctionInvokationAstNode intrinsicFunctionInvokationAstNode)
    {
        foreach (var arg in intrinsicFunctionInvokationAstNode.Args)
        {
            CheckExpression(arg);
        }

        return default;
    }

    protected virtual TR? CheckFunction(FunctionAstNode functionAstNode)
    {
        foreach (var arg in functionAstNode.Args)
        {
            CheckFunctionArg(arg);
        }

        foreach (var child in functionAstNode.Body.Children)
        {
            CheckAstNode(child);
        }

        return default;
    }
}
