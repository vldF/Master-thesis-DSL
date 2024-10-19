using Codegen.IR.Builder;
using Codegen.IR.nodes.expressions;
using Codegen.IR.nodes.types;
using Codegen.IR.Synthesizer.Tests.platform;
using NUnit.Framework;

namespace Codegen.IR.Synthesizer.Tests;

public class Tests : AbstractCodegenTest
{
    public Tests()
    {
        // set it to 'true' to override all expected test data to the actual ones
        UpdateTests = false;
    }

    [Test]
    public void SimpleTest()
    {
        var file = CodegenIrBuilder.CreateFile();
        var methodArgs = new Dictionary<string, ICgType>
        {
            {"arg1", CgSimpleType.IntType},
            {"arg2", CgSimpleType.IntType},
        };

        var method = file.CreateMethod("testMethod", methodArgs);
        var testVarOfInt = method.AddVarDecl("testVarOfInt", CgSimpleType.IntType);

        var intExpression = new CgIntLiteral(123);
        var initValue = new CgBinExpression(testVarOfInt, intExpression, CgBinExpression.BinOp.Plus);
        var testVarWithVarType = method.AddVarDecl("testVarWithVarType", init: initValue);

        method.AddReturn(testVarWithVarType.AsValue());

        Validate(file);
    }

    [Test]
    public void GenerateBuilderTest()
    {
        var file = CodegenIrBuilder.CreateFile();
        var methodBuilder = new CgVarExpression("Builder")
            .CallMethod("NewBuilder")
            .CallMethod("SetValue1", [CgIntLiteral.Const1])
            .CallMethod("SetValue2", [CgBoolLiteral.False]);

        file.Statements.Add(methodBuilder);
        Validate(file);
    }
}
