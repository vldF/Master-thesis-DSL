using Ast.Builder.exceptions;
using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.builder.exceptions;
using me.vldf.jsa.dsl.ir.builder.utils;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.declarations;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.nodes.statements;
using me.vldf.jsa.dsl.ir.references;
using me.vldf.jsa.dsl.parser;

namespace me.vldf.jsa.dsl.ir.builder.builder;

public class BaseBuilderVisitor(IrContext irContext) : JSADSLBaseVisitor<IAstNode>
{
    private readonly ExpressionBuilderVisitor _expessionBuilderVisitor = new(irContext);

    public override FileAstNode VisitFile(JSADSLParser.FileContext context)
    {
        var result = context
            .topLevelDecl()
            .Select(Visit)
            .Where(x => x is IStatementAstNode)
            .Cast<IStatementAstNode>()
            .ToList();

        return new FileAstNode(result);
    }

    public override FunctionAstNode VisitFuncDecl(JSADSLParser.FuncDeclContext context)
    {
        var name = context.name.Text;

        var newContext = new IrContext(irContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        var args = new List<FunctionArgAstNode>();
        var argIndex = 0;
        foreach (var argContext in context.functionArgs().functionArg())
        {
            var argName = argContext.name.Text;
            var argTypeName = argContext.type?.Text;
            var argType = argTypeName == null ? irContext.AnyTypeRef : new TypeReference(argTypeName, newContext);
            if (argType == null)
            {
                throw new UnresolvedTypeException(argTypeName!);
            }
            var arg = new FunctionArgAstNode(argName, argType, argIndex++);

            args.Add(arg);
            newContext.SaveNewArg(arg);
        }

        var resultTypeName = context.resultType?.Text;
        var resultTypeRef = resultTypeName != null
            ? new TypeReference(resultTypeName, newContext)
            : irContext.AnyTypeRef;

        if (resultTypeName != null && resultTypeRef == null)
        {
            throw new UnresolvedTypeException(resultTypeName);
        }

        var functionAstNode = new FunctionAstNode(
            name,
            args,
            resultTypeRef,
            newVisitor.VisitStatementsBlock(context.statementsBlock()));
        irContext.SaveNewFunc(functionAstNode);

        return functionAstNode;
    }

    public override IntrinsicFunctionAstNode VisitIntrinsicFuncDecl(JSADSLParser.IntrinsicFuncDeclContext context)
    {
        var name = context.name.Text;

        var args = new List<FunctionArgAstNode>();
        var argIndex = 0;
        foreach (var argContext in context.functionArgs().functionArg())
        {
            var argName = argContext.name.Text;
            var argTypeName = argContext.type?.Text;
            var argType = argTypeName == null ? irContext.AnyTypeRef : new TypeReference(argTypeName, irContext);
            if (argType == null)
            {
                throw new UnresolvedTypeException(argTypeName!);
            }
            var arg = new FunctionArgAstNode(argName, argType, argIndex++);

            args.Add(arg);
        }

        var resultTypeName = context.resultType?.Text;
        var resultTypeRef = resultTypeName == null
            ? irContext.AnyTypeRef
            : new TypeReference(resultTypeName, irContext);

        if (resultTypeName != null && resultTypeRef == null)
        {
            throw new UnresolvedTypeException(resultTypeName);
        }

        var functionAstNode = new IntrinsicFunctionAstNode(
            name,
            args,
            resultTypeRef);
        irContext.SaveNewFunc(functionAstNode);

        return functionAstNode;
    }

    public override IAstNode VisitObjectDecl(JSADSLParser.ObjectDeclContext context)
    {
        var newContext = new IrContext(irContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        var name = context.name.Text;
        var children = context.objectBody().children?.SelectNotNull(c => newVisitor.Visit(c)).ToList()!;

        var result = new ObjectAstNode(name, children);

        var type = new ObjectAstType(result);
        irContext.SaveNewType(type);

        return result;
    }

    public override IStatementAstNode VisitIfStatement(JSADSLParser.IfStatementContext context)
    {
        var cond = VisitExpression(context.cond);
        var mainBlockContext = new IrContext(irContext);
        var mainBlockVisitor = new BaseBuilderVisitor(mainBlockContext);

        var mainBlock = mainBlockVisitor.VisitStatementsBlock(context.mainBlock);
        IStatementAstNode? elseBranch = null;

        if (context.elseIfStatement != null)
        {
            var newContext = new IrContext(irContext);
            var newVisitor = new BaseBuilderVisitor(newContext);

            elseBranch = newVisitor.VisitIfStatement(context.elseIfStatement);
        } else if (context.elseBlock != null)
        {
            var newContext = new IrContext(irContext);
            var newVisitor = new BaseBuilderVisitor(newContext);

            elseBranch = newVisitor.VisitStatementsBlock(context.elseBlock);
        }

        return new IfStatementAstNode(cond, mainBlock, elseBranch);
    }

    public override StatementsBlockAstNode VisitStatementsBlock(JSADSLParser.StatementsBlockContext context)
    {
        var children = context.statement().SelectNotNull(VisitStatement).ToList();

        return new StatementsBlockAstNode(children);
    }

    public override IStatementAstNode VisitVarAssignmentStatement(JSADSLParser.VarAssignmentStatementContext context)
    {
        var varName = context.varName.Text;
        var varRef = new VariableReference(varName, irContext);

        var value = _expessionBuilderVisitor.VisitExpression(context.expression());

        return new VarAssignmentAstNode(varRef, value);
    }

    public override IStatementAstNode VisitVarDeclarationStatement(JSADSLParser.VarDeclarationStatementContext context)
    {
        var varName = context.varName.Text;
        var initValue = context.initValue;
        var typeName = context.type?.Text;

        var value = initValue != null ? _expessionBuilderVisitor.VisitExpression(initValue) : null;
        var type = typeName != null ? new TypeReference(typeName, irContext) : null;

        var varDeclarationStatement = new VarDeclAstNode(varName, type, value);
        irContext.SaveNewVar(varDeclarationStatement);

        return varDeclarationStatement;
    }

    public override IStatementAstNode VisitReturnStatement(JSADSLParser.ReturnStatementContext context)
    {
        var expressionContext = context.expression();
        if (expressionContext == null)
        {
            return new ReturnStatementAstNode(null);
        }

        var expression = VisitExpression(expressionContext);
        return new ReturnStatementAstNode(expression);
    }


    public override IExpressionAstNode VisitExpression(JSADSLParser.ExpressionContext context)
    {
        return _expessionBuilderVisitor.VisitExpression(context);
    }

    public override FunctionCallAstNode VisitFunctionCall(JSADSLParser.FunctionCallContext context)
    {
        var name = context.name.Text;
        var funcRef = new FunctionReference(name, irContext);
        var args = context.args
            .expression()
            .Select(argExpr => _expessionBuilderVisitor.VisitExpression(argExpr))
            .ToArray();

        return new FunctionCallAstNode(qualifiedParent: null, funcRef, args);
    }

    public override IStatementAstNode VisitStatement(JSADSLParser.StatementContext context)
    {
        if (context.ifStatement() != null)
        {
            return VisitIfStatement(context.ifStatement());
        }

        if (context.varAssignmentStatement() != null)
        {
            return VisitVarAssignmentStatement(context.varAssignmentStatement());
        }

        if (context.varDeclarationStatement() != null)
        {
            return VisitVarDeclarationStatement(context.varDeclarationStatement());
        }

        if (context.returnStatement() != null)
        {
            return VisitReturnStatement(context.returnStatement());
        }

        if (context.functionCall() != null)
        {
            return VisitFunctionCall(context.functionCall());
        }

        throw new ArgumentOutOfRangeException(nameof(context));
    }
}
