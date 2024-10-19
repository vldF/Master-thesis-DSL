using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.statements;

namespace me.vldf.jsa.dsl.ast.visitors;

public interface IAstVisitor
{
    public void VisitFileAstNode(FileAstNode node);
    public void VisitFunctionArgAstNode(FunctionArgAstNode node);
    public void VisitFunctionAstNode(FunctionAstNode node);
    public void VisitObjectAstNode(ObjectAstNode node);
    public void VisitVarDeclAstNode(VarDeclAstNode node);

    public void VisitExpressionAstNode(IExpressionAstNode node);
    public void VisitVarExpressionAstNode(VarExpressionAstNode node);

    public void VisitIfStatementAstNode(IfStatementAstNode node);
    public void VisitReturnStatementAstNode(ReturnStatementAstNode node);
    public void VisitStatementsBlockAstNode(StatementsBlockAstNode node);
    public void VisitVarAssignmentAstNode(VarAssignmentAstNode node);
}
