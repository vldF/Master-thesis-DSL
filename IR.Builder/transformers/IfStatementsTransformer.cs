using me.vldf.jsa.dsl.ir.builder.utils;
using me.vldf.jsa.dsl.ir.helpers;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class IfStatementsTransformer : AbstractAstSemanticTransformer
{
    protected override IStatementAstNode TransformIfStatementAstNode(IfStatementAstNode node)
    {
        var resultNodes = new List<IAstNode>();

        node = (IfStatementAstNode)base.TransformIfStatementAstNode(node);
        var conditionBool = SemanticsApi.Function("CreateCastToBoolOperator", node.Cond);
        var conditionBoolVarDecl = new VarDeclAstNode(GetFreshVar("condition"), null, conditionBool);
        resultNodes.Add(conditionBoolVarDecl);
        CurrentContext.SaveNewVar(conditionBoolVarDecl);

        var mainBranchStatements = TransformBranch(node.MainBlock, conditionBoolVarDecl.GetVarExpr(CurrentContext));
        resultNodes.AddRange(mainBranchStatements);

        if (node.ElseStatement == null)
        {
            return new StatementsBlockAstNode(resultNodes);
        }

        switch (node.ElseStatement)
        {
            case IfStatementAstNode elseIfStatement:
                var elseBranchStatements = (StatementsBlockAstNode)TransformIfStatementAstNode(elseIfStatement);
                resultNodes.AddRange(elseBranchStatements.Children);
                break;
            case StatementsBlockAstNode elseStatements:
                var notConditionBool = Interpretor.Function(
                    "InvokeFunction",
                    LocationArg,
                    SemanticsApi.Property("NotDescriptor"),
                    conditionBool);
                var notConditionBoolVarDecl = new VarDeclAstNode(
                    GetFreshVar("notCondition"),
                    null,
                    notConditionBool);

                resultNodes.Add(notConditionBoolVarDecl);
                CurrentContext.SaveNewVar(notConditionBoolVarDecl);

                resultNodes.AddRange(TransformBranch(elseStatements, notConditionBoolVarDecl.GetVarExpr(CurrentContext)));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node.ElseStatement));
        }

        return new StatementsBlockAstNode(resultNodes);
    }

    private List<IAstNode> TransformBranch(StatementsBlockAstNode body, IExpressionAstNode conditionBool)
    {
        var resultNodes = new List<IAstNode>();
        var conditionIdVarName = GetFreshVar("branchId");
        var fakeConditionVar = CurrentContext.GetFakeVariable(conditionIdVarName, isOutVar: true);
        var enterBranchCall = Engine.Function(
            "TryEnterBranch",
            conditionBool,
            fakeConditionVar);

        var leaveBranchCall = Engine.Function(
            "LeaveBranch",
            fakeConditionVar);

        resultNodes.Add(enterBranchCall);
        resultNodes.AddRange(body.Children);
        resultNodes.Add(leaveBranchCall);

        return resultNodes;
    }
}
