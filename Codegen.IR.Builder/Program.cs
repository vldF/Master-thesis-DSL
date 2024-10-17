using System.Linq.Expressions;
using Codegen.IR;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.statements;
using Codegen.IR.nodes.types;
using Codegen.Synthesizer;

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

        var expression1 = new CgBinExpression(
            method.GetArgument("arg1"),
            method.GetArgument("arg2"),
            CgBinExpression.BinOp.Plus);

        var varTestVar = new CgVarDeclStatement("testVar", SimpleType.IntType);
        method.Statements.Add(varTestVar);
        var assignment1 = new CgAssignmentStatement(varTestVar.AsValue(), expression1);
        method.Statements.Add(assignment1);
        var expression2 = new CgBinExpression(varTestVar.AsValue(), expression1, CgBinExpression.BinOp.Plus);
        var assignment2 = new CgAssignmentStatement(varTestVar.AsValue(), expression2);
        method.Statements.Add(assignment2);

        var returnStatement = new CgReturnStatement(varTestVar.AsValue());
        method.Statements.Add(returnStatement);

        Console.WriteLine(new CodegenSynthesizer().Synthesize(file));
    }
}
