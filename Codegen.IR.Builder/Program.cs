using System.Linq.Expressions;
using Codegen.IR;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using Codegen.IR.nodes.types;

namespace Codegen.IR.Builder;

public class Program
{
    public static void Main()
    {
        var file = CodegenIrBuilder.CreateFile("my-file.cs");

        var method = file.CreateMethod(
            "myMethod",
            new Dictionary<string, ICgType>
            {
                {"arg1", SimpleType.IntType},
                {"arg2", SimpleType.IntType},
            },
            SimpleType.IntType);

        var statement = new CgBinExpression(
            method.GetArgument("arg1"),
            method.GetArgument("arg2"),
            CgBinExpression.BinOp.Plus);

        var returnStatement = new CgReturnStatement(statement);
        method.Statements.Add(returnStatement);
    }
}
