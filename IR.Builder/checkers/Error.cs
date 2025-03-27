using me.vldf.jsa.dsl.ast.types;
using me.vldf.jsa.dsl.ir.nodes;
using me.vldf.jsa.dsl.ir.nodes.expressions;

namespace me.vldf.jsa.dsl.ir.builder.checkers;

public class Error(ErrorCode error, object[] args)
{
    public ErrorCode ErrorCode { get; } = error;
    public object[] Args { get; } = args;

    public static Error UnresolvedFunction(string name) => new(ErrorCode.UnresolvedFunction, [name]);
    public static Error UnresolvedType(string name) => new(ErrorCode.UnresolvedFunction, [name]);
    public static Error TypeMissmatch(AstType expected, AstType actual) => new(ErrorCode.TypeMissmatch, [expected, actual]);
    public static Error UnexpectedReturnExpression(IExpressionAstNode expr) => new(ErrorCode.TypeMissmatch, [expr]);
    public static Error NoReturnExpression() => new(ErrorCode.NoReturnExpression, []);
    public static Error NumericTypeExpected(AstType actual) => new(ErrorCode.NumericTypeExpected, [actual]);

    public string Format()
    {
        var messageTemplate = _messages.GetValueOrDefault(ErrorCode);
        if (messageTemplate == null)
        {
            throw new InvalidOperationException($"unknown error {ErrorCode}");
        }

        return string.Format(messageTemplate, args.Select(FormatArg).ToArray<object?>());
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
        { ErrorCode.TypeMissmatch, "Expected type {0} but got {1}" },
        { ErrorCode.UnexpectedReturnExpression, "Function {0} shouldn't return anythyng but it returns {1}" },
        { ErrorCode.NoReturnExpression, "Function {0} returns {1} but return without expression found" },
        { ErrorCode.NumericTypeExpected, "Expected arithmetic type but got {0}" },
    };
}

public enum ErrorCode
{
    UnresolvedFunction,
    UnresolvedType,
    TypeMissmatch,
    UnexpectedReturnExpression,
    NoReturnExpression,
    NumericTypeExpected,
}
