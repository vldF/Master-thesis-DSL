using System.Data;
using me.vldf.jsa.dsl.ast.nodes;
using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.statements;

namespace Ast.Builder.transformers;

public abstract class AbstractAstTransformer
{
    public virtual IAstNode Transform(IAstNode node)
    {
        return node switch
        {
            FileAstNode fileAstNode => TransformFileAstNode(fileAstNode),
            FunctionArgAstNode functionArgAstNode => TransformFunctionArgAstNode(functionArgAstNode),
            FunctionAstNode functionAstNode => TransformFunctionAstNode(functionAstNode),
            ObjectAstNode objectAstNode => TransformObjectAstNode(objectAstNode),
            VarDeclAstNode varDeclAstNode => TransformVarDeclAstNode(varDeclAstNode),
            IExpressionAstNode expressionAstNode => TransformExpressionAstNode(expressionAstNode),
            IStatementAstNode statementAstNode => TransformStatementAstNode(statementAstNode),
            _ => node
        };
    }

    protected virtual FileAstNode TransformFileAstNode(FileAstNode node) => node;

    protected virtual FunctionArgAstNode TransformFunctionArgAstNode(FunctionArgAstNode node) => node;

    protected virtual FunctionAstNode TransformFunctionAstNode(FunctionAstNode node) => node;

    protected virtual ObjectAstNode TransformObjectAstNode(ObjectAstNode node) => node;

    protected virtual VarDeclAstNode TransformVarDeclAstNode(VarDeclAstNode node) => node;

    protected virtual IExpressionAstNode TransformExpressionAstNode(IExpressionAstNode node)
    {
        return node switch
        {
            NewAstNode newAstNode => TransformNewAstNode(newAstNode),
            VarExpressionAstNode varExpressionAstNode => TransformVarExpressionAstNode(varExpressionAstNode),
            _ => node
        };
    }

    protected virtual VarExpressionAstNode TransformVarExpressionAstNode(VarExpressionAstNode node) => node;

    protected virtual IStatementAstNode TransformStatementAstNode(IStatementAstNode node)
    {
        return node switch
        {
            FunctionAstNode functionAstNode => TransformFunctionAstNode(functionAstNode),
            FunctionArgAstNode => throw new InvalidExpressionException("can't use argument as a statement"),
            ObjectAstNode objectAstNode => TransformObjectAstNode(objectAstNode),
            VarDeclAstNode varDeclAstNode => TransformVarDeclAstNode(varDeclAstNode),
            IfStatementAstNode ifStatementAstNode => TransformIfStatementAstNode(ifStatementAstNode),
            ReturnStatementAstNode returnStatementAstNode => TransformReturnStatementAstNode(returnStatementAstNode),
            StatementsBlockAstNode statementsBlockAstNode => TransformStatementsBlockAstNode(statementsBlockAstNode),
            VarAssignmentAstNode varAssignmentAstNode => TransformVarAssignmentAstNode(varAssignmentAstNode),
            _ => node
        };
    }

    protected virtual IfStatementAstNode TransformIfStatementAstNode(IfStatementAstNode node) => node;

    protected virtual ReturnStatementAstNode TransformReturnStatementAstNode(ReturnStatementAstNode node) => node;

    protected virtual StatementsBlockAstNode TransformStatementsBlockAstNode(StatementsBlockAstNode node) => node;

    protected virtual VarAssignmentAstNode TransformVarAssignmentAstNode(VarAssignmentAstNode node) => node;

    protected virtual NewAstNode TransformNewAstNode(NewAstNode node) => node;
}
