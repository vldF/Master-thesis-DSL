using Ast.Builder.exceptions;
using Ast.Builder.utils;
using me.vldf.jsa.dsl.ast.context;
using me.vldf.jsa.dsl.ast.nodes;
using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.statements;
using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.parser;

namespace Ast.Builder.builder;

public class BaseBuilderVisitor(AstContext astContext) : JSADSLBaseVisitor<IAstNode>
{
    private readonly ExpressionBuilderVisitor _expessionBuilderVisitor = new(astContext);

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

        var newContext = new AstContext(astContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        var args = new List<FunctionArgAstNode>();
        foreach (var argContext in context.functionArgs().functionArg())
        {
            var argName = argContext.name.Text;
            var argTypeName = argContext.type?.Text;
            var argType = argTypeName == null ? astContext.AnyType : astContext.ResolveType(argTypeName);
            if (argType == null)
            {
                throw new UnresolvedVariableException(argTypeName!);
            }
            var arg = new FunctionArgAstNode(argName, argType);

            args.Add(arg);
            newContext.SaveNewVar(arg);
        }

        var resultTypeName = context.resultType?.Text;
        var resultType = resultTypeName != null
            ? astContext.ResolveType(resultTypeName)
            : null;

        if (resultTypeName != null && resultType == null)
        {
            throw new UnresolvedTypeException(resultTypeName!);
        }

        return new FunctionAstNode(
            name,
            args,
            resultType,
            newVisitor.VisitStatementsBlock(context.statementsBlock()));
    }

    public override IAstNode VisitObjectDecl(JSADSLParser.ObjectDeclContext context)
    {
        var newContext = new AstContext(astContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        var name = context.name.Text;
        var children = context.objectBody().children?.SelectNotNull(c => newVisitor.Visit(c)).ToList()!;

        var result = new ObjectAstNode(name, children);

        var type = new ObjectAstType(result);
        astContext.SaveNewType(type);

        return result;
    }

    public override IAstNode VisitIfStatement(JSADSLParser.IfStatementContext context)
    {
        var cond = VisitExpression(context.cond);
        var mainBlockContext = new AstContext(astContext);
        var mainBlockVisitor = new BaseBuilderVisitor(mainBlockContext);

        var mainBlock = mainBlockVisitor.VisitStatementsBlock(context.mainBlock);
        IAstNode? elseStatement = null;

        if (context.else_if != null)
        {
            var newContext = new AstContext(astContext);
            var newVisitor = new BaseBuilderVisitor(newContext);

            elseStatement = newVisitor.VisitIfStatement(context.else_if)!;
        } else if (context.@else != null)
        {
            var newContext = new AstContext(astContext);
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
        var varDecl = astContext.ResolveVar(varName) ?? throw new UnresolvedVariableException(varName);

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
