using System.Data;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

public abstract class AbstractAstTransformer
{
    public virtual void Init(IrContext rootContext) { }

    public virtual IAstNode Transform(IAstNode node)
    {
        return node switch
        {
            FileAstNode fileAstNode => TransformFileAstNode(fileAstNode),
            IExpressionAstNode expressionAstNode => TransformExpressionAstNode(expressionAstNode),
            IStatementAstNode statementAstNode => TransformStatementAstNode(statementAstNode),
            _ => node
        };
    }

    protected virtual FileAstNode TransformFileAstNode(FileAstNode node)
    {
        node.TopLevelDeclarations = node.TopLevelDeclarations.Select(TransformStatementAstNode).ToList();

        return node;
    }

    protected virtual IStatementAstNode TransformFunctionArgAstNode(FunctionArgAstNode node)
    {
        return TransformVarDeclAstNode(node);
    }

    protected virtual FunctionAstNodeBase TransformFunctionAstNodeBase(FunctionAstNodeBase node)
    {
        return node switch
        {
            FunctionAstNode func => TransformFunctionAstNode(func),
            IntrinsicFunctionAstNode func => TransformIntrinsicFunctionAstNode(func),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node, null)
        };
    }

    protected virtual FunctionAstNode TransformFunctionAstNode(FunctionAstNode node)
    {
        node.Args = node.Args.Select(TransformStatementAstNode)
            .Select(it => (FunctionArgAstNode)it)
            .ToList();

        node.Body.Children = node.Body.Children.Select(Transform).ToList();

        return node;
    }

    protected virtual IntrinsicFunctionAstNode TransformIntrinsicFunctionAstNode(IntrinsicFunctionAstNode node)
    {
        node.Args = node.Args.Select(TransformStatementAstNode)
            .Select(it => (FunctionArgAstNode)it)
            .ToList();

        return node;
    }

    protected virtual ObjectAstNode TransformObjectAstNode(ObjectAstNode node)
    {
        node.Children = node.Children.Select(Transform).ToList();

        return node;
    }

    protected virtual VarDeclAstNode TransformVarDeclAstNode(VarDeclAstNode node)
    {
        if (node.Init != null)
        {
            node.Init = TransformExpressionAstNode(node.Init);
        }

        return node;
    }

    protected virtual IExpressionAstNode TransformExpressionAstNode(IExpressionAstNode node)
    {
        return node switch
        {
            NewAstNode newAstNode => TransformNewAstNode(newAstNode),
            VarExpressionAstNode varExpressionAstNode => TransformVarExpressionAstNode(varExpressionAstNode),
            BinaryExpressionAstNode binaryExpressionAstNode => TransforBinaryAstNode(binaryExpressionAstNode),
            UnaryExpressionAstNode unaryExpressionAstNode => TransforUnaryAstNode(unaryExpressionAstNode),
            FunctionCallAstNode functionCallAstNode => TransformFunctionCallAstNode(functionCallAstNode),
            _ => node
        };
    }

    protected virtual IExpressionAstNode TransformFunctionCallAstNode(FunctionCallAstNode node)
    {
        var copy = node.Copy<FunctionCallAstNode>();
        copy.Args = copy.Args.Select(TransformExpressionAstNode).ToArray();

        return copy;
    }

    protected virtual IExpressionAstNode TransforBinaryAstNode(BinaryExpressionAstNode node)
    {
        return node with
        {
            Left = TransformExpressionAstNode(node.Left),
            Right = TransformExpressionAstNode(node.Right),
        };
    }

    protected virtual IExpressionAstNode TransforUnaryAstNode(UnaryExpressionAstNode node)
    {
        return node with
        {
            Value = TransformExpressionAstNode(node.Value)
        };
    }

    protected virtual VarExpressionAstNode TransformVarExpressionAstNode(VarExpressionAstNode node)
    {
        return node;
    }

    protected virtual IStatementAstNode TransformStatementAstNode(IStatementAstNode node)
    {
        return node switch
        {
            FunctionAstNodeBase functionAstNodeBase => TransformFunctionAstNodeBase(functionAstNodeBase),
            FunctionArgAstNode functionArgAstNode => TransformFunctionArgAstNode(functionArgAstNode),
            ObjectAstNode objectAstNode => TransformObjectAstNode(objectAstNode),
            VarDeclAstNode varDeclAstNode => TransformVarDeclAstNode(varDeclAstNode),
            IfStatementAstNode ifStatementAstNode => TransformIfStatementAstNode(ifStatementAstNode),
            ReturnStatementAstNode returnStatementAstNode => TransformReturnStatementAstNode(returnStatementAstNode),
            StatementsBlockAstNode statementsBlockAstNode => TransformStatementsBlockAstNode(statementsBlockAstNode),
            VarAssignmentAstNode varAssignmentAstNode => TransformVarAssignmentAstNode(varAssignmentAstNode),
            _ => node
        };
    }

    protected virtual IStatementAstNode TransformIfStatementAstNode(IfStatementAstNode node)
    {
        node.MainBlock = TransformStatementsBlockAstNode(node.MainBlock);
        if (node.ElseStatement != null)
        {
            node.ElseStatement = TransformStatementAstNode(node.ElseStatement);
        }

        node.Cond = TransformExpressionAstNode(node.Cond);
        return node;
    }

    protected virtual ReturnStatementAstNode TransformReturnStatementAstNode(ReturnStatementAstNode node)
    {
        if (node.Expression != null)
        {
            node.Expression = TransformExpressionAstNode(node.Expression);
        }

        return node;
    }

    protected virtual StatementsBlockAstNode TransformStatementsBlockAstNode(StatementsBlockAstNode node)
    {
        node.Children = node.Children.Select(Transform).ToList();

        return node;
    }

    protected virtual VarAssignmentAstNode TransformVarAssignmentAstNode(VarAssignmentAstNode node)
    {
        node.Value = TransformExpressionAstNode(node.Value);
        return node;
    }

    protected virtual NewAstNode TransformNewAstNode(NewAstNode node)
    {
        return node with
        {
            Args = node.Args.Select(TransformExpressionAstNode).ToList()
        };
    }
}
