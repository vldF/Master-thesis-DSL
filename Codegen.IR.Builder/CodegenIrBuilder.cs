using Codegen.IR.nodes;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using Codegen.IR.nodes.types;

namespace Codegen.IR.Builder;

public static class CodegenIrBuilder
{
    public static CgFile CreateFile(string name = "")
    {
        var file = new CgFile(name);
        return file;
    }

    public static CgMethod CreateMethod(
        this ICgStatementsContainer statementsContainer,
        string name,
        Dictionary<string, ICgType> args,
        ICgType? returnType = null,
        ICollection<CgAnnotation>? annotations = null)
    {
        var method = new CgMethod(name, args, returnType ?? CgSimpleType.VoidType, annotations ?? []);
        statementsContainer.Statements.Add(method);

        return method;
    }

    public static CgVarDeclStatement AddVarDecl(
        this ICgStatementsContainer statementsContainer,
        string name,
        ICgType? type = null,
        ICgExpression? init = null)
    {
        var varDecl = new CgVarDeclStatement(name, type, init);
        statementsContainer.Statements.Add(varDecl);
        return varDecl;
    }

    public static CgAssignmentStatement AddAssignment(
        this ICgStatementsContainer statementsContainer,
        ICgExpression left,
        ICgExpression right)
    {
        var statement = new CgAssignmentStatement(left, right);
        statementsContainer.Statements.Add(statement);

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
        IReadOnlyCollection<ICgExpression>? args = null,
        IReadOnlyCollection<ICgType>? generics = null)
    {
        return new CgMethodCall(reciever, methodName, args ?? [], generics);
    }

    public static CgValueWithReciever Property(this ICgExpression reciever,
        string propertyName)
    {
        return new CgValueWithReciever(reciever, propertyName);
    }

    public static CgValueWithIndex Index(this ICgExpression reciever, int idx)
    {
        return new CgValueWithIndex(reciever, idx);
    }

    public static ICgExpression VarDeclaration(
        ICgStatementsContainer statementsContainer,
        string name,
        ICgType? type = null,
        ICgExpression? init = null)
    {
        var varDecl = new CgVarDeclStatement(name, Type: type, Init: init);
        statementsContainer.Statements.Add(varDecl);

        return varDecl.AsValue();
    }

    public static CgAssignmentStatement VarAssignment(
        ICgStatementsContainer statementsContainer,
        string varName,
        ICgExpression value)
    {
        var variable = new CgVarExpression(varName);
        var varAssignment = new CgAssignmentStatement(variable, value);
        statementsContainer.Statements.Add(varAssignment);

        return varAssignment;
    }

    public static ICgExpression AsExpression<T>(T value)
    {
        return value switch
        {
            int i => new CgIntLiteral(i),
            double d => new CgFloatLiteral(d),
            float f => new CgFloatLiteral(f),
            string str => new CgStringLiteral(str),
            bool b => new CgBoolLiteral(b),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
