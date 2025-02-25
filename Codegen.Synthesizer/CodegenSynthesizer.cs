using System.Globalization;
using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using Codegen.IR.nodes.types;

namespace Codegen.Synthesizer;

public class CodegenSynthesizer : AbstractTextSynthesizer
{
    public string Synthesize(CgFile file)
    {
        SynthFile(file);

        return ToString();
    }

    private void SynthFile(CgFile file)
    {
        foreach (var statement in file.Statements)
        {
            SynthStatement(statement);
        }
    }

    private void SynthStatement(ICgStatement statement)
    {
        switch (statement)
        {
            case CgMethod cgMethod:
                SynthMethodDecl(cgMethod);
                break;
            case CgMethodCall cgMethodCall:
                SynthMethodCallStatement(cgMethodCall);
                break;
            case CgIfElseStatement cgIfElseStatement:
                SynthIfElseStatement(cgIfElseStatement);
                break;
            case CgReturnStatement cgReturnStatement:
                SynthReturnStatement(cgReturnStatement);
                break;
            case CgUsingStatement cgUsingStatement:
                break;
            case CgVarDeclStatement cgVarDeclStatement:
                SynthVarDecl(cgVarDeclStatement);
                break;
            case CgForeachStatement foreachStatement:
                break;
            case CgForStatement forStatement:
                break;
            case CgAssignmentStatement assignmentStatement:
                SynthAssignmentStatement(assignmentStatement);
                break;
            case CgDirectiveStatement directiveStatement:
                SynthDirectiveStatement(directiveStatement);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(statement));
        }
    }

    private void SynthDirectiveStatement(CgDirectiveStatement directiveStatement)
    {
        Append("#");
        Append(directiveStatement.Name);
        AppendSpace();
        Append("\"");
        Append(directiveStatement.Argument);
        AppendLine("\"");
    }

    private void SynthIfElseStatement(CgIfElseStatement cgIfElseStatement)
    {
        Append("if (");
        SynthExpression(cgIfElseStatement.Cond);
        AppendLine(")");
        AppendLine("{");
        IncreaserIndent();

        foreach (var mainBodyStatement in cgIfElseStatement.MainBody.Statements)
        {
            SynthStatement(mainBodyStatement);
        }

        DecreaseIndent();
        Append("}");

        if (cgIfElseStatement.Elseif != null)
        {
            AppendLine(" else ");
            SynthIfElseStatement(cgIfElseStatement.Elseif);
        }

        if (cgIfElseStatement.ElseBody != null)
        {
            AppendLine(" else {");
            IncreaserIndent();

            foreach (var elseBodyStatement in cgIfElseStatement.ElseBody.Statements)
            {
                SynthStatement(elseBodyStatement);
            }

            DecreaseIndent();
            AppendLine("}");
        }
    }

    private void SynthAssignmentStatement(CgAssignmentStatement assignmentStatement)
    {
        SynthExpression(assignmentStatement.Left);
        AppendSpace();
        Append("=");
        AppendSpace();
        SynthExpression(assignmentStatement.Right);
        AppendSemicolon();
    }

    private void SynthReturnStatement(CgReturnStatement cgReturnStatement)
    {
        if (cgReturnStatement.Value == null)
        {
            Append("return");
        }
        else
        {
            Append("return ");
            SynthExpression(cgReturnStatement.Value);
        }

        AppendSemicolon();
    }

    private void SynthMethodDecl(CgMethod method)
    {
        SynthTypeRef(method.ReturnType);

        AppendSpace();
        Append(method.Name);
        Append("(");

        var lastMethodArg = method.ArgTypes.LastOrDefault();
        foreach (var (argName, argType) in method.ArgTypes.SkipLast(1))
        {
            SynthTypeRef(argType);
            AppendSpace();
            Append(argName);
            Append(", ");
        }

        if (method.ArgTypes.Count > 0)
        {
            SynthTypeRef(lastMethodArg.Value);
            AppendSpace();
            Append(lastMethodArg.Key);
        }

        AppendLine(")");
        AppendLine("{");
        IncreaserIndent();

        foreach (var methodStatement in method.Statements)
        {
            SynthStatement(methodStatement);
        }

        DecreaseIndent();
        AppendLine("}");
    }

    private void SynthTypeRef(ICgType type)
    {
        switch (type)
        {
            case CgSimpleType simpleType:
                Append(simpleType.Name);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }

    private void SynthVarDecl(CgVarDeclStatement declStatement)
    {
        if (declStatement.Type != null)
        {
            SynthTypeRef(declStatement.Type);
        }
        else
        {
            Append("var");
        }

        AppendSpace();
        Append(declStatement.Name);

        if (declStatement.Init != null)
        {
            AppendSpace();
            Append("=");
            AppendSpace();

            SynthExpression(declStatement.Init);
        }

        AppendSemicolon();
    }


    private void SynthExpression(ICgExpression expression)
    {
        switch (expression)
        {
            case CgMethodCall cgMethodCall:
                SynthMethodCall(cgMethodCall);
                break;
            case CgArrayIndexExpression cgArrayIndexExpression:
                SynthArrayExpression(cgArrayIndexExpression);
                break;
            case CgBinExpression cgBinExpression:
                SynthBinExpression(cgBinExpression);
                break;
            case CgUnaryExpression cgUnaryExpression:
                SynthUnExpression(cgUnaryExpression);
                break;
            case CgBoolLiteral cgBoolExpression:
                Append(cgBoolExpression.Value ? "true" : "false");
                break;
            case CgIntLiteral cgIntExpression:
                Append(cgIntExpression.Value.ToString());
                break;
            case CgFloatLiteral cgFloatExpression:
                Append(cgFloatExpression.Value.ToString(CultureInfo.InvariantCulture));
                break;
            case CgStringLiteral cgStringExpression:
                Append(cgStringExpression.Value);
                break;
            case CgVarExpression cgVarExpression:
                SynthVarExpression(cgVarExpression);
                break;
            case CgValueWithReciever cgValueWithReciever:
                SynthValueWithReciever(cgValueWithReciever);
                break;
            case CgValueWithIndex cgValueWithIndex:
                SynthValueWithIndex(cgValueWithIndex);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(expression));
        }
    }

    private void SynthUnExpression(CgUnaryExpression cgUnaryExpression)
    {
        switch (cgUnaryExpression.Operation)
        {
            case CgUnOp.Minus:
                Append("-");
                break;
            case CgUnOp.Not:
                Append("!");
                break;
            default:
                throw new ArgumentOutOfRangeException(cgUnaryExpression.Operation.ToString());
        }

        Append("(");
        SynthExpression(cgUnaryExpression.Value);
        Append(")");
    }

    private void SynthValueWithIndex(CgValueWithIndex cgValueWithIndex)
    {
        SynthExpression(cgValueWithIndex.Reciever);
        Append("[");
        Append(cgValueWithIndex.idx.ToString());
        Append("]");
    }

    private void SynthValueWithReciever(CgValueWithReciever cgValueWithReciever)
    {
        SynthExpression(cgValueWithReciever.Reciever);
        Append(".");
        Append(cgValueWithReciever.Name);
    }

    private void SynthArrayExpression(CgArrayIndexExpression cgArrayIndexExpression)
    {
        SynthExpression(cgArrayIndexExpression.Reciever);
        Append("[");
        SynthExpression(cgArrayIndexExpression.Index);
        Append("]");
        AppendSemicolon();
    }

    private void SynthMethodCallStatement(CgMethodCall methodCall)
    {
        SynthMethodCall(methodCall);
        AppendSemicolon();
    }

    private void SynthMethodCall(CgMethodCall methodCall)
    {
        if (methodCall.Reciever != null)
        {
            SynthExpression(methodCall.Reciever);
            Append(".");
        }
        Append(methodCall.Name);
        Append("(");

        var lastArg = methodCall.Args.LastOrDefault();
        foreach (var methodCallArg in methodCall.Args.SkipLast(1))
        {
            SynthExpression(methodCallArg);
            Append(", ");
        }

        if (lastArg != null)
        {
            SynthExpression(lastArg);
        }

        Append(")");
    }

    private void SynthBinExpression(CgBinExpression expression)
    {
        Append("(");

        SynthExpression(expression.Left);
        AppendSpace();

        switch (expression.Operation)
        {
            case CgBinExpression.BinOp.Sum:
                Append("+");
                break;
            case CgBinExpression.BinOp.Sub:
                Append("-");
                break;
            case CgBinExpression.BinOp.Mul:
                Append("*");
                break;
            case CgBinExpression.BinOp.Div:
                Append("/");
                break;
            case CgBinExpression.BinOp.Eq:
                Append("==");
                break;
            case CgBinExpression.BinOp.NotEq:
                Append("!=");
                break;
            case CgBinExpression.BinOp.Lt:
                Append("<");
                break;
            case CgBinExpression.BinOp.Gt:
                Append(">");
                break;
            case CgBinExpression.BinOp.LtEq:
                Append("<=");
                break;
            case CgBinExpression.BinOp.GtEq:
                Append(">=");
                break;
            case CgBinExpression.BinOp.AndAnd:
                Append("&&");
                break;
            case CgBinExpression.BinOp.OrOr:
                Append("||");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        AppendSpace();
        SynthExpression(expression.Right);

        Append(")");
    }

    private void SynthVarExpression(CgVarExpression varExpression)
    {
        if (varExpression.isOutVar)
        {
            Append("out var ");
        }
        Append(varExpression.Name);
    }
}
