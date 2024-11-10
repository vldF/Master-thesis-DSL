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
            var argType = argTypeName == null ? irContext.AnyTypeRef : new TypeReference(argTypeName);
            if (argType == null)
            {
                throw new UnresolvedVariableException(argTypeName!);
            }
            var arg = new FunctionArgAstNode(argName, argType, argIndex++);

            args.Add(arg);
            newContext.SaveNewVar(arg);
        }

        var resultTypeName = context.resultType?.Text;
        var resultTypeRef = resultTypeName != null
            ? new TypeReference(resultTypeName)
            : irContext.AnyTypeRef;

        if (resultTypeName != null && resultTypeRef == null)
        {
            throw new UnresolvedTypeException(resultTypeName!);
        }

        return new FunctionAstNode(
            name,
            args,
            resultTypeRef,
            newVisitor.VisitStatementsBlock(context.statementsBlock()));
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

    public override IAstNode VisitIfStatement(JSADSLParser.IfStatementContext context)
    {
        var cond = VisitExpression(context.cond);
        var mainBlockContext = new IrContext(irContext);
        var mainBlockVisitor = new BaseBuilderVisitor(mainBlockContext);

        var mainBlock = mainBlockVisitor.VisitStatementsBlock(context.mainBlock);
        IAstNode? elseStatement = null;

        if (context.else_if != null)
        {
            var newContext = new IrContext(irContext);
            var newVisitor = new BaseBuilderVisitor(newContext);

            elseStatement = newVisitor.VisitIfStatement(context.else_if)!;
        } else if (context.@else != null)
        {
            var newContext = new IrContext(irContext);
            var newVisitor = new BaseBuilderVisitor(newContext);

            elseStatement = newVisitor.VisitStatementsBlock(context.@else);
        }

        return new IfStatementAstNode(cond, mainBlock, elseStatement);
    }

    public override StatementsBlockAstNode VisitStatementsBlock(JSADSLParser.StatementsBlockContext context)
    {
        var children = context.statement().SelectNotNull(Visit).ToList();

        return new StatementsBlockAstNode(children);
    }

    public override IAstNode VisitVarAssignmentStatement(JSADSLParser.VarAssignmentStatementContext context)
    {
        var varName = context.varName.Text;
        var varRef = new VariableReference(varName);
        var varDecl = irContext.ResolveVar(varRef) ?? throw new UnresolvedVariableException(varName);

        var value = _expessionBuilderVisitor.VisitExpression(context.expression());

        return new VarAssignmentAstNode(varDecl, value);
    }

    public override IAstNode VisitReturnStatement(JSADSLParser.ReturnStatementContext context)
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
}
