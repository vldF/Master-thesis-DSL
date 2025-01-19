using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ast.visitors;

public interface IAstVisitor
{
    public void Visit(IAstNode node)
    {
        switch (node)
        {
            case FileAstNode fileAstNode:
                VisitFileAstNode(fileAstNode);
                break;
            case FunctionArgAstNode functionArgAstNode:
                VisitFunctionArgAstNode(functionArgAstNode);
                break;
            case FunctionAstNode functionAstNode:
                VisitFunctionAstNode(functionAstNode);
                break;
            case ObjectAstNode objectAstNode:
                VisitObjectAstNode(objectAstNode);
                break;
            case VarDeclAstNode varDeclAstNode:
                VisitVarDeclAstNode(varDeclAstNode);
                break;
            case VarExpressionAstNode varExpressionAstNode:
                VisitVarExpressionAstNode(varExpressionAstNode);
                break;
            case IExpressionAstNode expressionAstNode:
                VisitExpressionAstNode(expressionAstNode);
                break;
            case IfStatementAstNode ifStatementAstNode:
                VisitIfStatementAstNode(ifStatementAstNode);
                break;
            case ReturnStatementAstNode returnStatementAstNode:
                VisitReturnStatementAstNode(returnStatementAstNode);
                break;
            case StatementsBlockAstNode statementsBlockAstNode:
                VisitStatementsBlockAstNode(statementsBlockAstNode);
                break;
            case VarAssignmentAstNode varAssignmentAstNode:
                VisitVarAssignmentAstNode(varAssignmentAstNode);
                break;
            case IStatementAstNode statementAstNode:
                VisitStatementAstNode(statementAstNode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node));
        }
    }
    public void VisitFileAstNode(FileAstNode node);
    protected void VisitFunctionArgAstNode(FunctionArgAstNode node);
    protected void VisitFunctionAstNode(FunctionAstNode node);
    protected void VisitObjectAstNode(ObjectAstNode node);
    protected void VisitVarDeclAstNode(VarDeclAstNode node);

    protected void VisitExpressionAstNode(IExpressionAstNode node);
    protected void VisitVarExpressionAstNode(VarExpressionAstNode node);

    public void VisitStatementAstNode(IStatementAstNode node)
    {
        switch (node)
        {
            case FunctionAstNode functionAstNode:
                VisitFunctionAstNode(functionAstNode);
                break;
            case FunctionArgAstNode functionArgAstNode:
                throw new InvalidOperationException("can't use argument as a statement");
            case ObjectAstNode objectAstNode:
                VisitObjectAstNode(objectAstNode);
                break;
            case VarDeclAstNode varDeclAstNode:
                VisitVarDeclAstNode(varDeclAstNode);
                break;
            case IfStatementAstNode ifStatementAstNode:
                VisitIfStatementAstNode(ifStatementAstNode);
                break;
            case ReturnStatementAstNode returnStatementAstNode:
                VisitReturnStatementAstNode(returnStatementAstNode);
                break;
            case StatementsBlockAstNode statementsBlockAstNode:
                VisitStatementsBlockAstNode(statementsBlockAstNode);
                break;
            case VarAssignmentAstNode varAssignmentAstNode:
                VisitVarAssignmentAstNode(varAssignmentAstNode);
                break;
            case IntrinsicFunctionAstNode intrinsicFunctionAstNode:
                VisitIntrinsicFunctionAstNode(intrinsicFunctionAstNode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node));
        }
    }

    protected void VisitIfStatementAstNode(IfStatementAstNode node);
    protected void VisitReturnStatementAstNode(ReturnStatementAstNode node);
    protected void VisitStatementsBlockAstNode(StatementsBlockAstNode node);
    protected void VisitVarAssignmentAstNode(VarAssignmentAstNode node);
    protected void VisitIntrinsicFunctionAstNode(IntrinsicFunctionAstNode intrinsicFunctionAstNode);
}
