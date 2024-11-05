using Ast.Builder.exceptions;
using me.vldf.jsa.dsl.ast.context;
using me.vldf.jsa.dsl.ast.nodes;
using me.vldf.jsa.dsl.ast.nodes.expressions;
using me.vldf.jsa.dsl.parser;

namespace Ast.Builder.builder;

public class ExpressionBuilderVisitor(AstContext astContext) : JSADSLBaseVisitor<IExpressionAstNode>
{
    public override IExpressionAstNode VisitVariableExpression(JSADSLParser.VariableExpressionContext context)
    {
        var name = context.name.Text;
        var variableDecl = astContext.ResolveVar(name) ?? throw new UnresolvedVariableException(name);

        return new VarExpressionAstNode(variableDecl);
    }

    public override IExpressionAstNode VisitNewExpression(JSADSLParser.NewExpressionContext context)
    {
        var name = context.name.Text;
        var args = context
            .expressionList()
            .expression()
            .Select(VisitExpression)
            .ToList();

        return new NewAstNode(name, args);
    }
}
