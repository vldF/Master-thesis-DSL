using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.builder.checkers;

public class Error(ErrorCode error, object[] args)
{
    public ErrorCode ErrorCode { get; } = error;
    public object[] Args { get; } = args;

    public static Error UnresolvedFunction(string name) => new(ErrorCode.UnresolvedFunction, [name]);
    public static Error UnresolvedType(string name) => new(ErrorCode.UnresolvedType, [name]);
    public static Error UnresolvedVar(string name) => new(ErrorCode.UnresolvedVar, [name]);
    public static Error TypeMissmatch(AstType expected, AstType actual) => new(ErrorCode.TypeMissmatch, [expected, actual]);
    public static Error UnexpectedReturnExpression(IExpressionAstNode expr) => new(ErrorCode.TypeMissmatch, [expr]);
    public static Error NoReturnExpression() => new(ErrorCode.NoReturnExpression, []);
    public static Error NumericTypeExpected(AstType actual) => new(ErrorCode.NumericTypeExpected, [actual]);
    public static Error UnexpectedGenericsCount(int expected, int actual) => new(ErrorCode.UnexpectedGenericsCount, [expected, actual]);
    public static Error CanNotInferReturnType(string funcName) => new(ErrorCode.CanNotInferReturnType, [funcName]);
    public static Error CanNotInferVarType(string varName) => new(ErrorCode.CanNotInferVarType, [varName]);
    public static Error RecieverTypeCanNotBeGeneric(AstType type) => new(ErrorCode.RecieverTypeCanNotBeGeneric, [type]);
    public static Error CanNotResolveRecieverType(AstType type) => new(ErrorCode.CanNotResolveRecieverType, [type]);

    // python specific
    public static Error InitFuncCanNotHaveReturn(string objectName) => new(ErrorCode.InitFuncCanNotHaveReturn, [objectName]);
    public static Error InitFuncCanReturnOnlyObjectType(string objectName) => new(ErrorCode.InitFuncCanReturnOnlyObjectType, [objectName]);

    public string Format()
    {
        var messageTemplate = _messages.GetValueOrDefault(ErrorCode);
        if (messageTemplate == null)
        {
            throw new InvalidOperationException($"unknown error {ErrorCode}");
        }

        return string.Format(messageTemplate, Args.Select(FormatArg).ToArray<object?>());
    }

    private string FormatArg(object arg)
    {
        return arg switch
        {
            AstType astType => astType.Name,
            IAstNode astNode => astNode.String(),
            _ => arg.ToString()!
        };
    }

    public override bool Equals(object? obj)
    {
        return obj is Error errorDescriptor && Equals(errorDescriptor);
    }

    public bool Equals(Error other)
    {
        return ErrorCode == other.ErrorCode && Args.Equals(other.Args);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)ErrorCode, Args);
    }

    private static readonly Dictionary<ErrorCode, string> _messages = new()
    {
        { ErrorCode.UnresolvedFunction, "Unresolved function {0}" },
        { ErrorCode.UnresolvedType, "Unresolved type {0}" },
        { ErrorCode.UnresolvedVar, "Unresolved var {0}" },
        { ErrorCode.TypeMissmatch, "Expected type {0} but got {1}" },
        { ErrorCode.UnexpectedReturnExpression, "Function {0} shouldn't return anythyng but it returns {1}" },
        { ErrorCode.NoReturnExpression, "Function {0} returns {1} but return without expression found" },
        { ErrorCode.NumericTypeExpected, "Expected arithmetic type but got {0}" },
        { ErrorCode.UnexpectedGenericsCount, "Expected {0} type parameters but got {1}" },
        { ErrorCode.CanNotInferReturnType, "Can't infer return type of function {0}" },
        { ErrorCode.CanNotInferVarType, "Can't infer type of variable {0}" },
        { ErrorCode.RecieverTypeCanNotBeGeneric, "reciever can not have generic type {0}" },
        { ErrorCode.CanNotResolveRecieverType, "reciever type {0} is unresolved" },
        { ErrorCode.InitFuncCanNotHaveReturn, "init function can not have return statement, but it have in object {0}" },
        { ErrorCode.InitFuncCanReturnOnlyObjectType, "init function must have no return type or its type must match its object, but it is violated for {0}" },
    };
}

public enum ErrorCode
{
    UnresolvedFunction,
    UnresolvedType,
    UnresolvedVar,
    TypeMissmatch,
    UnexpectedReturnExpression,
    NoReturnExpression,
    NumericTypeExpected,
    UnexpectedGenericsCount,
    CanNotInferReturnType,
    CanNotInferVarType,
    RecieverTypeCanNotBeGeneric,
    CanNotResolveRecieverType,
    InitFuncCanNotHaveReturn,
    InitFuncCanReturnOnlyObjectType,
}
