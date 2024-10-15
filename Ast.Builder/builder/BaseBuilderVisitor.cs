using Ast.Builder.exceptions;
using Ast.Builder.utils;
using me.vldf.jsa.dsl.ast.context;
using me.vldf.jsa.dsl.ast.nodes;
using me.vldf.jsa.dsl.ast.nodes.declarations;
using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.ast.nodes.statements;
using me.vldf.jsa.dsl.parser;

namespace Ast.Builder.builder;

public class BaseBuilderVisitor(AstContext astContext) : JSADSLBaseVisitor<AstNode>
{
    private readonly ExpressionBuilderVisitor _expessionBuilderVisitor = new(astContext);

    public override FileAstNode VisitFile(JSADSLParser.FileContext context)
    {
        var result = context.topLevelDecl().Select(Visit).ToList();

        return new FileAstNode(result);
    }

    public override FunctionAstNode VisitFuncDecl(JSADSLParser.FuncDeclContext context)
    {
        var name = context.name.Text;

        var newContext = new AstContext(astContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        foreach (var argContext in context.args().arg())
        {
            var argName = argContext.name.Text;
            var argType = argContext.type?.Text;
            var arg = new FunctionArgFakeDeclAstNode(argName, argType);

            newContext.SaveNewVar(arg);
        }

        return new FunctionAstNode(name, newVisitor.VisitStatementsBlock(context.statementsBlock()));
    }

    public override AstNode VisitObjectDecl(JSADSLParser.ObjectDeclContext context)
    {
        var newContext = new AstContext(astContext);
        var newVisitor = new BaseBuilderVisitor(newContext);

        var name = context.name.Text;
        var children = context.objectBody().children?.SelectNotNull(c => newVisitor.Visit(c)).ToList()!;

        return new ObjectAstNode(name, children);
    }

    public override AstNode VisitIfStatement(JSADSLParser.IfStatementContext context)
    {
        var cond = VisitExpression(context.cond);
        var mainBlockContext = new AstContext(astContext);
        var mainBlockVisitor = new BaseBuilderVisitor(mainBlockContext);

        var mainBlock = mainBlockVisitor.VisitStatementsBlock(context.mainBlock);
        AstNode? elseStatement = null;

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

    public override AstNode VisitVarAssignmentStatement(JSADSLParser.VarAssignmentStatementContext context)
    {
        var varName = context.varName.Text;
        var varDecl = astContext.ResolveVar(varName) ?? throw new UnresolvedVariableException(varName);

        var value = _expessionBuilderVisitor.VisitExpression(context.expression());

        return new VarAssignmentAstNode(varDecl, value);
    }

    public override AstNode VisitReturnStatement(JSADSLParser.ReturnStatementContext context)
    {
        var expressionContext = context.expression();
        if (expressionContext == null)
        {
            return new ReturnStatementAstNode(null);
        }

        var expression = VisitExpression(expressionContext);
        return new ReturnStatementAstNode(expression);
    }


    public override ExpressionAstNode VisitExpression(JSADSLParser.ExpressionContext context)
    {
        return _expessionBuilderVisitor.VisitExpression(context);
    }
}
