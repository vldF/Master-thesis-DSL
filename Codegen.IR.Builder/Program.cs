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
                {"arg1", CgSimpleType.IntType},
                {"arg2", CgSimpleType.IntType},
            },
            CgSimpleType.IntType);

        var expression1 = new CgBinExpression(
            method.GetArgument("arg1"),
            method.GetArgument("arg2"),
            CgBinExpression.BinOp.Plus);

        var varTestVar = method.AddVarDecl("testVar", CgSimpleType.IntType);

        method.AddAssignment(varTestVar.AsValue(), expression1);
        var expression2 = new CgBinExpression(
            varTestVar.AsValue(),
            expression1,
            CgBinExpression.BinOp.Plus);

        method.AddAssignment(varTestVar.AsValue(), expression2);

        method.AddReturn(varTestVar.AsValue());

        Console.WriteLine(new CodegenSynthesizer().Synthesize(file));
    }
}
