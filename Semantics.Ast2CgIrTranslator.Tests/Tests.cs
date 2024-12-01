using Codegen.Synthesizer;
using me.vldf.jsa.dsl.ir.builder.builder;
using NUnit.Framework;
using TestPlatform;

namespace Semantics.Ast2CgIrTranslator.Tests;

public class Tests : CodegenTestBase
{
    [SetUp]
    public void Setup()
    {
        UpdateTests = false;
    }

    private static IEnumerable<TestCaseData> GetAllTests()
    {
        return Directory
            .EnumerateFiles(_testDataProvider.GetInputDirPath())
            .Select(Path.GetFileNameWithoutExtension)
            .Select(fileName => new TestCaseData(fileName)
            {
                TestName = fileName
            });
    }

    [TestCaseSource(nameof(GetAllTests))]
    public void Test(string testName)
    {
        var inputPath = _testDataProvider.GetInputCodePath(testName);
        var inputCode = File.ReadAllText(inputPath);

        var astBuilder = new AstBuilder();
        var file = astBuilder.FromString(inputCode);
        Console.WriteLine(file.String());

        var translator = new Ast2IrTranslator();
        var cgFile = translator.Translate(file);

        Validate(cgFile);
    }
}
