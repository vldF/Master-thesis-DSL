using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using Codegen.IR.nodes.types;

namespace Codegen.IR.Builder;

public static class CodegenIrBuilder
{
    public static CgFile CreateFile(string name = "")
    {
        return new CgFile(name);
    }

    public static CgMethod CreateMethod(this CgFile file, string name, Dictionary<string, ICgType> args, ICgType? returnType = null)
    {
        var method = new CgMethod(name, args, returnType ?? CgSimpleType.VoidType);
        file.Statements.Add(method);

        return method;
    }

    public static CgVarDeclStatement AddVarDecl(this CgMethod method, string name, ICgType? type = null, ICgExpression? init = null)
    {
        var varDecl = new CgVarDeclStatement(name, type, init);
        method.Statements.Add(varDecl);
        return varDecl;
    }

    public static CgAssignmentStatement AddAssignment(this CgMethod method, ICgExpression left, ICgExpression right)
    {
        var statement = new CgAssignmentStatement(left, right);
        method.Statements.Add(statement);

        return statement;
    }

    public static CgReturnStatement AddReturn(this CgMethod method, ICgExpression? expression = null)
    {
        var statement = new CgReturnStatement(expression);
        method.Statements.Add(statement);

        return statement;
    }

    public static CgMethodCall CallMethod(this ICgExpression reciever,
        string methodName,
        IReadOnlyCollection<ICgExpression>? args = null)
    {
        return new CgMethodCall(reciever, methodName, args ?? []);
    }

    public static ICgExpression VarDeclaration(
        this CgFile file,
        string name,
        ICgType? type = null,
        ICgExpression? init = null)
    {
        var globalVar = new CgVarDeclStatement(name, Type: null, Init: init);
        file.Statements.Add(globalVar);

        return globalVar.AsValue();
    }

    public static ICgExpression AsExpression<T>(T value)
    {
        return value switch
        {
            int i => new CgIntLiteral(i),
            string str => new CgStringExpression(str),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
