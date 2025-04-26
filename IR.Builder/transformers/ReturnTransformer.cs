using me.vldf.jsa.dsl.ir.builder.transformers.utils;
using me.vldf.jsa.dsl.ir.builder.utils;
using me.vldf.jsa.dsl.ir.helpers;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public class ReturnTransformer : AbstractAstSemanticTransformer
{
    private VariableReference _currentReturnConditionalBuilder = null!;
    private ReturnStatementAstNode? _newReturn = null!;

    protected override FunctionAstNode TransformFunctionAstNode(FunctionAstNode node)
    {
        var returnNodesCount = GetReturnsCount(node);
        return returnNodesCount switch
        {
            0 => ProcessZeroReturnsCase(node),
            1 => node,
            _ => ProcessMultipleReturnsCase(node)
        };
    }

    private FunctionAstNode ProcessZeroReturnsCase(FunctionAstNode node)
    {
        var children = node.Body.Children.ToList();
        node.Body.Children = children;

        var newReturn = new ReturnStatementAstNode(SemanticsApi.Property("None"))
        {
            Parent = node
        };
        children.Insert(children.Count, newReturn);

        return node;
    }

    private FunctionAstNode ProcessMultipleReturnsCase(FunctionAstNode node)
    {
        var children = node.Body.Children.ToList();
        var returnValueBuilderCall = Interpretor.Function("CreateReturnedValuesBuilder");

        var builderVarName = GetFreshVar("returnValueBuilder");
        var returnValueBuilderDecl = new VarDeclAstNode(builderVarName, null, returnValueBuilderCall);
        node.Context.SaveNewVar(returnValueBuilderDecl);
        var builderVarRef = returnValueBuilderDecl.GetVarRef(node.Context);
        _currentReturnConditionalBuilder = builderVarRef;

        children.Insert(0, returnValueBuilderDecl);

        _newReturn = new ReturnStatementAstNode(
            _currentReturnConditionalBuilder.Resolve()!.GetVarExpr(node.Context).Function("ToConditional"));
        children.Insert(children.Count, _newReturn);

        node.Body.Children = children;

        return base.TransformFunctionAstNode(node);
    }

    protected override IStatementAstNode TransformReturnStatementAstNode(ReturnStatementAstNode node)
    {
        if (ReferenceEquals(node, _newReturn))
        {
            // we do not transform new return node
            return node;
        }

        var currentCond = Interpretor
            .Property("CurrentContext")
            .Property("CurrentCondition");
        var currentCondVarName = GetFreshVar("currentCond");
        var currentCondVarDecl = new VarDeclAstNode(currentCondVarName, null, currentCond);
        var context = ((IContextOwner)node.Parent!).Context;
        context.SaveNewVar(currentCondVarDecl);
        var currentCondVarExpr = currentCondVarDecl.GetVarExpr(context);

        var returnExpr = node.Expression ?? SemanticsApi.Property("None");

        var addOptionInvoke = new VarExpressionAstNode(_currentReturnConditionalBuilder)
            .Function(
                "AddOption",
                currentCondVarExpr,
                returnExpr);

        List<IAstNode> newNodes = [currentCondVarDecl, addOptionInvoke];
        return new StatementsBlockAstNode(newNodes)
        {
            Context = context,
            Parent = node.Parent
        };
    }

    private int GetReturnsCount(FunctionAstNode node)
    {
        return new NodeFinder<ReturnStatementAstNode>().FindAllNodesOfType(node).Count;
    }
}
