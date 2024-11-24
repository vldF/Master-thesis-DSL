using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;

namespace me.vldf.jsa.dsl.ir.builder.transformers;

/// <summary>
///     Set SealedValue in all references to the current value
/// </summary>
public class ReferenceSealer : AbstractAstTransformer
{
    protected override FileAstNode TransformFileAstNode(FileAstNode node)
    {
        var file = base.TransformFileAstNode(node).Copy<FileAstNode>();
        file.TopLevelDeclarations = file.TopLevelDeclarations.Select(TransformStatementAstNode).ToList();

        return file;
    }

    protected override FunctionAstNode TransformFunctionAstNode(FunctionAstNode node)
    {
        var func = base.TransformFunctionAstNode(node).Copy<FunctionAstNode>();
        if (func.ReturnTypeRef != null)
        {
            func.ReturnTypeRef.SealedValue = func.ReturnTypeRef.Resolve();
        }

        for (var i = func.Args.Count - 1; i >= 0; i--)
        {
            func.Args[i] = TransformFunctionArgAstNode(func.Args[i]);
        }

        func.Body = TransformStatementsBlockAstNode(func.Body);

        return func;
    }

    protected override FunctionArgAstNode TransformFunctionArgAstNode(FunctionArgAstNode node)
    {
        var arg = base.TransformFunctionArgAstNode(node).Copy<FunctionArgAstNode>();
        arg.TypeReference = arg.TypeReference.Clone<TypeReference>();
        arg.TypeReference.SealedValue = arg.TypeReference.Resolve();

        return arg;
    }

    protected override StatementsBlockAstNode TransformStatementsBlockAstNode(StatementsBlockAstNode node)
    {
        var block = base.TransformStatementsBlockAstNode(node).Copy<StatementsBlockAstNode>();
        var children = block.Children.Select(Transform);
        block.Children = children.ToList();
        return block;
    }

    protected override ObjectAstNode TransformObjectAstNode(ObjectAstNode node)
    {
        var @object = base.TransformObjectAstNode(node).Copy<ObjectAstNode>();
        @object.Children = @object.Children.Select(Transform).ToList();
        return @object;
    }

    protected override VarDeclAstNode TransformVarDeclAstNode(VarDeclAstNode node)
    {
        var varDecl = base.TransformVarDeclAstNode(node).Copy<VarDeclAstNode>();
        varDecl.TypeReference.SealedValue = varDecl.TypeReference.Resolve();

        if (varDecl.Init != null)
        {
            varDecl.Init = TransformExpressionAstNode(varDecl.Init);
        }

        return varDecl;
    }

    protected override VarExpressionAstNode TransformVarExpressionAstNode(VarExpressionAstNode node)
    {
        var varExpression = base.TransformVarExpressionAstNode(node).Copy<VarExpressionAstNode>();
        varExpression.VariableReference.SealedValue = varExpression.VariableReference.Resolve();

        return varExpression;
    }

    protected override BinaryExpressionAstNode TransforBinaryAstNode(BinaryExpressionAstNode node)
    {
        var binExpression = (BinaryExpressionAstNode)base.TransforBinaryAstNode(node) with
        {
            Left = TransformExpressionAstNode(node.Left),
            Right = TransformExpressionAstNode(node.Right),
        };

        return binExpression;
    }

    protected override UnaryExpressionAstNode TransforUnaryAstNode(UnaryExpressionAstNode node)
    {
        return (UnaryExpressionAstNode)base.TransforUnaryAstNode(node) with
        {
            Value = TransformExpressionAstNode(node.Value),
        };
    }

    protected override IfStatementAstNode TransformIfStatementAstNode(IfStatementAstNode node)
    {
        var ifStatement = base.TransformIfStatementAstNode(node).Copy<IfStatementAstNode>();
        ifStatement.Cond = TransformExpressionAstNode(ifStatement.Cond);
        ifStatement.MainBlock = TransformStatementsBlockAstNode(ifStatement.MainBlock);
        if (ifStatement.ElseBlock != null)
        {
            ifStatement.ElseBlock = TransformStatementsBlockAstNode(ifStatement.ElseBlock);
        }

        return ifStatement;
    }

    protected override ReturnStatementAstNode TransformReturnStatementAstNode(ReturnStatementAstNode node)
    {
        var returnStatement = base.TransformReturnStatementAstNode(node).Copy<ReturnStatementAstNode>();
        if (returnStatement.Expression != null)
        {
            returnStatement.Expression = TransformExpressionAstNode(returnStatement.Expression);
        }

        return returnStatement;
    }

    protected override VarAssignmentAstNode TransformVarAssignmentAstNode(VarAssignmentAstNode node)
    {
        var assignment = base.TransformVarAssignmentAstNode(node).Copy<VarAssignmentAstNode>();
        assignment.VariableReference.SealedValue = assignment.VariableReference.Resolve();
        assignment.Value = TransformExpressionAstNode(assignment.Value);

        return assignment;
    }

    protected override NewAstNode TransformNewAstNode(NewAstNode node)
    {
        node.TypeReference.SealedValue = base.TransformNewAstNode(node).TypeReference.Resolve();

        return node with
        {
            Args = node.Args.Select(TransformExpressionAstNode).ToList()
        };
    }
}
