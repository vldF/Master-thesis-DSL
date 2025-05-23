using me.vldf.jsa.dsl.ir.context;
using me.vldf.jsa.dsl.ir.nodes.expressions;
using me.vldf.jsa.dsl.ir.references;
using me.vldf.jsa.dsl.parser;

namespace me.vldf.jsa.dsl.ir.builder.builder;

public class ExpressionBuilderVisitor(IrContext irContext) : JSADSLBaseVisitor<IExpressionAstNode>
{
    public override IExpressionAstNode VisitExpression(JSADSLParser.ExpressionContext context)
    {
        if (context.L_PAREN() != null)
        {
            return VisitExpression(context.expr_in_paren);
        }

        if (context.functionCall() != null)
        {
            return VisitFunctionCall(context.functionCall());
        }

        if (context.op == null)
        {
            return VisitExpressionAtomic(context.atomic);
        }

        if (context.unary != null)
        {
            var value = context.unary;
            return context.op.Text switch
            {
                "-" => VisitUnartOp(value, UnaryOperation.MINUS),
                "!" => VisitUnartOp(value, UnaryOperation.NOT),
                _ => throw new InvalidOperationException($"unsupported{context.op.Text}")
            };
        }

        var left = context.left;
        var right = context.right;
        return context.op.Text switch
        {
            "*" => VisitBinaryOp(left, right, BinaryOperation.Mul),
            "/" => VisitBinaryOp(left, right, BinaryOperation.Div),
            "%" => VisitBinaryOp(left, right, BinaryOperation.Mod),
            "+" => VisitBinaryOp(left, right, BinaryOperation.Sum),
            "-" => VisitBinaryOp(left, right, BinaryOperation.Sub),
            "==" => VisitBinaryOp(left, right, BinaryOperation.Eq),
            "!=" => VisitBinaryOp(left, right, BinaryOperation.NotEq),
            "<=" => VisitBinaryOp(left, right, BinaryOperation.LtEq),
            "<" => VisitBinaryOp(left, right, BinaryOperation.Lt),
            ">=" => VisitBinaryOp(left, right, BinaryOperation.GtEq),
            ">" => VisitBinaryOp(left, right, BinaryOperation.Gt),
            "&&" => VisitBinaryOp(left, right, BinaryOperation.AndAnd),
            "||" => VisitBinaryOp(left, right, BinaryOperation.OrOr),
            "^" => VisitBinaryOp(left, right, BinaryOperation.Xor),
            _ => throw new InvalidOperationException($"unsupported{context.op.Text}")
        };
    }

    private IExpressionAstNode VisitUnartOp(JSADSLParser.ExpressionContext value, UnaryOperation op)
    {
        var valueExpr = VisitExpression(value);
        return new UnaryExpressionAstNode(valueExpr, op);
    }

    private IExpressionAstNode VisitBinaryOp(
        JSADSLParser.ExpressionContext left,
        JSADSLParser.ExpressionContext right,
        BinaryOperation op)
    {
        var leftExpr = VisitExpression(left);
        var rightExpr = VisitExpression(right);

        return new BinaryExpressionAstNode(leftExpr, rightExpr, op);
    }

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

    public override IExpressionAstNode VisitFloatNumberLiteral(JSADSLParser.FloatNumberLiteralContext context)
    {
        return new FloatLiteralAstNode(double.Parse(context.GetText()));
    }

    public override IExpressionAstNode VisitIntegerNumberLiteral(JSADSLParser.IntegerNumberLiteralContext context)
    {
        return new IntLiteralAstNode(int.Parse(context.GetText()));
    }

    public override IExpressionAstNode VisitBoolLiteral(JSADSLParser.BoolLiteralContext context)
    {
        return new BoolLiteralAstNode(bool.Parse(context.GetText()));
    }

    public override IExpressionAstNode VisitStringLiteral(JSADSLParser.StringLiteralContext context)
    {
        var quotedString = context.DoubleQuotedString().GetText();
        return new StringLiteralAstNode(quotedString[1..^1]);
    }

    public override FunctionCallAstNode VisitFunctionCall(JSADSLParser.FunctionCallContext context)
    {
        var qualifiedAccess = VisitQualifiedAccess(context.qualifiedAccess());
        string funcName;
        switch (qualifiedAccess)
        {
            case QualifiedAccessPropertyAstNode qa:
                funcName = qa.PropertyName;
                qualifiedAccess = qa.QualifiedParent;
                break;
            case VarExpressionAstNode varExpressionAstNode:
                funcName = varExpressionAstNode.VariableReference.Name;
                qualifiedAccess = null;
                break;
            default:
                throw new InvalidOperationException();
        }

        var functionReference = new FunctionReference(funcName, irContext);

        var args = context.args
            .expression()
            .Select(VisitExpression)
            .ToArray();
        var generics = context
            .generic()
            ?.ID()
            ?.Select(id => new TypeReference(id.GetText(), irContext))
            .ToArray() ?? [];


        return new FunctionCallAstNode(qualifiedParent: qualifiedAccess, functionReference, generics, args);
    }

    public override IExpressionAstNode VisitQualifiedAccess(JSADSLParser.QualifiedAccessContext context)
    {
        var baseVarName = context.ID()[0].GetText();
        var @base = new VarExpressionAstNode(new VariableReference(baseVarName, irContext));
        IExpressionAstNode result = @base;
        foreach (var entry in context.ID().Skip(1))
        {
            result = new QualifiedAccessPropertyAstNode(result, entry.GetText());
        }

        return result;
    }
}
