using Ast.Builder.exceptions;
using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;
using me.vldf.jsa.dsl.parser;

namespace me.vldf.jsa.dsl.ir.builder.builder;

public class ExpressionBuilderVisitor(IrContext irContext) : JSADSLBaseVisitor<IExpressionAstNode>
{
    public override IExpressionAstNode VisitVariableExpression(JSADSLParser.VariableExpressionContext context)
    {
        var name = context.name.Text;
        var varRef = new VariableReference(name, irContext);
        return new VarExpressionAstNode(varRef);
    }

    public override IExpressionAstNode VisitNewExpression(JSADSLParser.NewExpressionContext context)
    {
        var typeName = context.name.Text;
        var typeRef = new TypeReference(typeName, irContext);
        var args = context
            .expressionList()
            .expression()
            .Select(VisitExpression)
            .ToList();

        return new NewAstNode(typeRef, args);
    }
}
