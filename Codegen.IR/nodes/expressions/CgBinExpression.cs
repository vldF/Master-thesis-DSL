using Codegen.IR.nodes.statements;

namespace Codegen.IR.nodes.expressions;

public record CgBinExpression(
    ICgExpression Left,
    ICgExpression Right,
    CgBinExpression.BinOp Operation) : ICgExpression
{
    public CgBinExpression(CgVarDeclStatement Left,
        ICgExpression Right,
        BinOp Operation) : this(Left.AsValue(), Right, Operation) { }

    public CgBinExpression(ICgExpression Left,
        CgVarDeclStatement Right,
        BinOp Operation) : this(Left, Right.AsValue(), Operation) { }

    public CgBinExpression(CgVarDeclStatement Left,
        CgVarDeclStatement Right,
        BinOp Operation) : this(Left, Right.AsValue(), Operation) { }

    public enum BinOp
    {
        Plus, Minus, Mul, Div, Eq, NotEq, Less, Great, LessEq, GreatEq,
        And, Or
    }

}
