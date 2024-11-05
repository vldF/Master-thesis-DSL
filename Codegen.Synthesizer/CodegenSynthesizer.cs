using System.Text;
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
            default:
                throw new ArgumentOutOfRangeException(nameof(statement));
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
            case CgBoolLiteral cgBoolExpression:
                Append(cgBoolExpression.Value ? "true" : "false");
                break;
            case CgIntLiteral cgIntExpression:
                Append(cgIntExpression.Value.ToString());
                break;
            case CgStringExpression cgStringExpression:
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
        SynthExpression(expression.Left);
        AppendSpace();

        switch (expression.Operation)
        {
            case CgBinExpression.BinOp.Plus:
                Append("+");
                break;
            case CgBinExpression.BinOp.Minus:
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
            case CgBinExpression.BinOp.Less:
                Append("<");
                break;
            case CgBinExpression.BinOp.Great:
                Append(">");
                break;
            case CgBinExpression.BinOp.LessEq:
                Append("<=");
                break;
            case CgBinExpression.BinOp.GreatEq:
                Append(">=");
                break;
            case CgBinExpression.BinOp.And:
                Append("&&");
                break;
            case CgBinExpression.BinOp.Or:
                Append("||");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        AppendSpace();
        SynthExpression(expression.Right);
    }

    private void SynthVarExpression(CgVarExpression varExpression)
    {
        Append(varExpression.Name);
    }
}
