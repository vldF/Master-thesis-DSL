using Codegen.IR.Builder;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;
using Codegen.IR.Synthesizer.Tests.platform;
using NUnit.Framework;

namespace Codegen.IR.Synthesizer.Tests;

public class Tests : AbstractCodegenTest
{
    [Test]
    public void SimpleTest()
    {
        var file = CodegenIrBuilder.CreateFile();
        var methodArgs = new Dictionary<string, ICgType>
        {
            {"arg1", SimpleType.IntType},
            {"arg2", SimpleType.IntType},
        };

        var method = file.CreateMethod("testMethod", methodArgs);
        var testVarOfInt = method.AddVarDecl("testVarOfInt", SimpleType.IntType);

        var intExpression = new CgIntExpression(123);
        var initValue = new CgBinExpression(testVarOfInt, intExpression, CgBinExpression.BinOp.Plus);
        var testVarWithVarType = method.AddVarDecl("testVarWithVarType", init: initValue);

        method.AddReturn(testVarWithVarType.AsValue());

        Validate(file);
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
